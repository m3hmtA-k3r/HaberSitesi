/**
 * SERVICE WORKER
 * Offline support and caching for WebUI News Site
 */

const CACHE_NAME = 'masker-news-v1';
const OFFLINE_URL = '/offline.html';

// Files to cache immediately
const PRECACHE_URLS = [
    '/',
    '/Home',
    '/Haberler',
    '/css/style.css',
    '/css/mobile.css',
    '/js/main.js',
    '/js/mobile.js',
    '/lib/owlcarousel/owl.carousel.min.js',
    '/lib/owlcarousel/assets/owl.carousel.min.css',
    OFFLINE_URL
];

// Install event - cache essential files
self.addEventListener('install', (event) => {
    console.log('[ServiceWorker] Install');

    event.waitUntil(
        caches.open(CACHE_NAME).then((cache) => {
            console.log('[ServiceWorker] Caching app shell');
            return cache.addAll(PRECACHE_URLS);
        }).then(() => {
            return self.skipWaiting();
        })
    );
});

// Activate event - clean up old caches
self.addEventListener('activate', (event) => {
    console.log('[ServiceWorker] Activate');

    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames.map((cacheName) => {
                    if (cacheName !== CACHE_NAME) {
                        console.log('[ServiceWorker] Removing old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        }).then(() => {
            return self.clients.claim();
        })
    );
});

// Fetch event - serve from cache, fallback to network
self.addEventListener('fetch', (event) => {
    // Skip cross-origin requests
    if (!event.request.url.startsWith(self.location.origin)) {
        return;
    }

    // Skip non-GET requests
    if (event.request.method !== 'GET') {
        return;
    }

    event.respondWith(
        caches.match(event.request).then((response) => {
            if (response) {
                console.log('[ServiceWorker] Serving from cache:', event.request.url);
                return response;
            }

            return fetch(event.request).then((response) => {
                // Don't cache invalid responses
                if (!response || response.status !== 200 || response.type !== 'basic') {
                    return response;
                }

                // Clone the response
                const responseToCache = response.clone();

                caches.open(CACHE_NAME).then((cache) => {
                    // Cache API responses and images
                    if (event.request.url.includes('/api/') ||
                        event.request.url.match(/\.(jpg|jpeg|png|gif|webp|svg)$/)) {
                        cache.put(event.request, responseToCache);
                    }
                });

                return response;
            }).catch(() => {
                // Offline fallback
                if (event.request.mode === 'navigate') {
                    return caches.match(OFFLINE_URL);
                }

                // Fallback for images
                if (event.request.destination === 'image') {
                    return new Response(
                        '<svg width="100" height="100" xmlns="http://www.w3.org/2000/svg"><rect width="100" height="100" fill="#ddd"/><text x="50%" y="50%" text-anchor="middle" dy=".3em" fill="#999">Offline</text></svg>',
                        { headers: { 'Content-Type': 'image/svg+xml' } }
                    );
                }
            });
        })
    );
});

// Background sync for offline actions
self.addEventListener('sync', (event) => {
    console.log('[ServiceWorker] Background sync:', event.tag);

    if (event.tag === 'sync-comments') {
        event.waitUntil(syncComments());
    }
});

// Push notifications
self.addEventListener('push', (event) => {
    console.log('[ServiceWorker] Push received');

    const options = {
        body: event.data ? event.data.text() : 'Yeni haber bildirimi',
        icon: '/img/icon-192x192.png',
        badge: '/img/badge-72x72.png',
        vibrate: [200, 100, 200],
        tag: 'news-notification',
        requireInteraction: false,
        actions: [
            {
                action: 'open',
                title: 'Haberi Aç',
                icon: '/img/open-icon.png'
            },
            {
                action: 'close',
                title: 'Kapat',
                icon: '/img/close-icon.png'
            }
        ]
    };

    event.waitUntil(
        self.registration.showNotification('Masker News', options)
    );
});

// Notification click handler
self.addEventListener('notificationclick', (event) => {
    console.log('[ServiceWorker] Notification click:', event.action);

    event.notification.close();

    if (event.action === 'open') {
        event.waitUntil(
            clients.openWindow('/')
        );
    }
});

// Helper function to sync offline comments
async function syncComments() {
    try {
        const cache = await caches.open('offline-comments');
        const requests = await cache.keys();

        for (const request of requests) {
            const response = await cache.match(request);
            const data = await response.json();

            // Send to server
            await fetch('/api/Yorum/InsertYorum', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            // Remove from cache after successful sync
            await cache.delete(request);
        }

        return Promise.resolve();
    } catch (error) {
        console.error('[ServiceWorker] Sync failed:', error);
        return Promise.reject(error);
    }
}

// Message handler from clients
self.addEventListener('message', (event) => {
    console.log('[ServiceWorker] Message received:', event.data);

    if (event.data.action === 'skipWaiting') {
        self.skipWaiting();
    }

    if (event.data.action === 'clearCache') {
        event.waitUntil(
            caches.delete(CACHE_NAME).then(() => {
                return self.registration.update();
            })
        );
    }
});

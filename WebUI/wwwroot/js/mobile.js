/**
 * MOBILE OPTIMIZATION JAVASCRIPT
 * WebUI News Site - Mobile Features
 */

(function ($) {
    "use strict";

    // ========== DEVICE DETECTION ==========
    const isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    const isIOS = /iPad|iPhone|iPod/.test(navigator.userAgent);
    const isAndroid = /Android/.test(navigator.userAgent);

    // ========== VIEWPORT HEIGHT FIX (iOS) ==========
    function setViewportHeight() {
        let vh = window.innerHeight * 0.01;
        document.documentElement.style.setProperty('--vh', `${vh}px`);
    }

    if (isMobile) {
        setViewportHeight();
        window.addEventListener('resize', setViewportHeight);
        window.addEventListener('orientationchange', setViewportHeight);
    }

    // ========== LAZY LOADING IMAGES ==========
    function lazyLoadImages() {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    const src = img.getAttribute('data-src');
                    if (src) {
                        img.src = src;
                        img.removeAttribute('data-src');
                        img.classList.add('loaded');
                    }
                    observer.unobserve(img);
                }
            });
        }, {
            rootMargin: '50px'
        });

        document.querySelectorAll('img[data-src]').forEach(img => {
            imageObserver.observe(img);
        });
    }

    // ========== TOUCH GESTURES ==========
    let touchStartX = 0;
    let touchEndX = 0;
    let touchStartY = 0;
    let touchEndY = 0;

    function handleSwipe() {
        const swipeThreshold = 50;
        const diffX = touchEndX - touchStartX;
        const diffY = touchEndY - touchStartY;

        // Horizontal swipe
        if (Math.abs(diffX) > Math.abs(diffY) && Math.abs(diffX) > swipeThreshold) {
            if (diffX > 0) {
                // Swipe right
                onSwipeRight();
            } else {
                // Swipe left
                onSwipeLeft();
            }
        }

        // Vertical swipe
        if (Math.abs(diffY) > Math.abs(diffX) && Math.abs(diffY) > swipeThreshold) {
            if (diffY > 0) {
                // Swipe down
                onSwipeDown();
            } else {
                // Swipe up
                onSwipeUp();
            }
        }
    }

    function onSwipeLeft() {
        // Navigate to next news (if on detail page)
        console.log('Swipe left detected');
        showSwipeIndicator('Sonraki haber');
    }

    function onSwipeRight() {
        // Navigate to previous news or go back
        console.log('Swipe right detected');
        showSwipeIndicator('Önceki haber');
        // Optional: Go back in history
        // if (document.referrer) {
        //     window.history.back();
        // }
    }

    function onSwipeDown() {
        // Pull to refresh
        console.log('Swipe down detected');
    }

    function onSwipeUp() {
        // Hide navbar on scroll up
        console.log('Swipe up detected');
    }

    // Add swipe indicator
    function showSwipeIndicator(text) {
        let indicator = document.querySelector('.swipe-indicator');
        if (!indicator) {
            indicator = document.createElement('div');
            indicator.className = 'swipe-indicator';
            document.body.appendChild(indicator);
        }
        indicator.textContent = text;
        indicator.classList.add('show');
        setTimeout(() => {
            indicator.classList.remove('show');
        }, 1500);
    }

    // Touch event listeners
    if (isMobile) {
        document.addEventListener('touchstart', e => {
            touchStartX = e.changedTouches[0].screenX;
            touchStartY = e.changedTouches[0].screenY;
        }, { passive: true });

        document.addEventListener('touchend', e => {
            touchEndX = e.changedTouches[0].screenX;
            touchEndY = e.changedTouches[0].screenY;
            handleSwipe();
        }, { passive: true });
    }

    // ========== PULL TO REFRESH ==========
    let pullStartY = 0;
    let pullMoveY = 0;
    let isPulling = false;
    const pullThreshold = 80;

    function initPullToRefresh() {
        const pullIndicator = $('<div class="pull-to-refresh"><i class="fa fa-sync-alt"></i> Yenilemek için çekin</div>');
        $('body').prepend(pullIndicator);

        $(document).on('touchstart', function (e) {
            if (window.scrollY === 0) {
                pullStartY = e.touches[0].clientY;
                isPulling = true;
            }
        });

        $(document).on('touchmove', function (e) {
            if (isPulling) {
                pullMoveY = e.touches[0].clientY - pullStartY;
                if (pullMoveY > 0 && pullMoveY < pullThreshold * 2) {
                    pullIndicator.css('top', (pullMoveY - 60) + 'px');
                }
                if (pullMoveY > pullThreshold) {
                    pullIndicator.addClass('active');
                    pullIndicator.html('<i class="fa fa-sync-alt fa-spin"></i> Bırakın ve yenileyin');
                }
            }
        });

        $(document).on('touchend', function () {
            if (isPulling && pullMoveY > pullThreshold) {
                // Refresh page
                pullIndicator.html('<i class="fa fa-sync-alt fa-spin"></i> Yenileniyor...');
                setTimeout(() => {
                    location.reload();
                }, 500);
            } else {
                pullIndicator.removeClass('active');
                pullIndicator.css('top', '-60px');
            }
            isPulling = false;
            pullMoveY = 0;
        });
    }

    // ========== SMART NAVBAR HIDE/SHOW ==========
    let lastScrollTop = 0;
    const navbar = $('.navbar');
    const delta = 5;

    function smartNavbar() {
        const scrollTop = $(window).scrollTop();

        if (Math.abs(lastScrollTop - scrollTop) <= delta) {
            return;
        }

        if (scrollTop > lastScrollTop && scrollTop > navbar.outerHeight()) {
            // Scrolling down
            navbar.addClass('nav-up').removeClass('nav-down');
        } else {
            // Scrolling up
            if (scrollTop + $(window).height() < $(document).height()) {
                navbar.addClass('nav-down').removeClass('nav-up');
            }
        }

        lastScrollTop = scrollTop;
    }

    // ========== LOADING INDICATOR ==========
    function showLoading() {
        const loader = $('<div class="mobile-loading active"><div class="spinner"></div></div>');
        $('body').append(loader);
    }

    function hideLoading() {
        $('.mobile-loading').removeClass('active').fadeOut(300, function () {
            $(this).remove();
        });
    }

    // ========== OPTIMIZE IMAGES FOR MOBILE ==========
    function optimizeImages() {
        $('img').each(function () {
            const $img = $(this);
            const src = $img.attr('src');

            // Add loading=lazy attribute
            if (!$img.attr('loading')) {
                $img.attr('loading', 'lazy');
            }

            // Convert to data-src for lazy loading
            if (src && !$img.attr('data-src')) {
                $img.attr('data-src', src);
                $img.attr('src', 'data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7');
            }
        });
    }

    // ========== IMPROVE FORM INPUTS ==========
    function improveFormInputs() {
        // Auto-focus prevention on iOS
        if (isIOS) {
            $('input, textarea').on('focus', function () {
                $(this).attr('readonly', 'readonly');
                $(this).removeAttr('readonly');
            });
        }

        // Add input type validation
        $('input[type="email"]').attr('inputmode', 'email');
        $('input[type="tel"]').attr('inputmode', 'tel');
        $('input[type="number"]').attr('inputmode', 'numeric');

        // Prevent zoom on input focus (iOS)
        $('input, textarea, select').css('font-size', '16px');
    }

    // ========== TOUCH-FRIENDLY DROPDOWNS ==========
    function improveTouchTargets() {
        // Increase touch targets
        $('.badge, .btn-sm, .nav-link').css({
            'min-height': '44px',
            'min-width': '44px',
            'padding': '12px 16px'
        });

        // Add visual feedback on touch
        $('a, button, .btn').on('touchstart', function () {
            $(this).addClass('touch-active');
        }).on('touchend touchcancel', function () {
            $(this).removeClass('touch-active');
        });
    }

    // ========== PREVENT DOUBLE TAP ZOOM ==========
    function preventDoubleTapZoom() {
        let lastTouchEnd = 0;
        document.addEventListener('touchend', function (e) {
            const now = Date.now();
            if (now - lastTouchEnd <= 300) {
                e.preventDefault();
            }
            lastTouchEnd = now;
        }, { passive: false });
    }

    // ========== SMOOTH SCROLL ==========
    function smoothScroll() {
        $('a[href*="#"]').not('[href="#"]').click(function (e) {
            if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') &&
                location.hostname == this.hostname) {
                let target = $(this.hash);
                target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
                if (target.length) {
                    e.preventDefault();
                    $('html, body').animate({
                        scrollTop: target.offset().top - 70
                    }, 800);
                }
            }
        });
    }

    // ========== MOBILE MENU AUTO CLOSE ==========
    function autoCloseMenu() {
        $('.navbar-nav .nav-link').on('click', function () {
            if ($('.navbar-toggler').is(':visible')) {
                $('.navbar-collapse').collapse('hide');
            }
        });

        // Close menu when clicking outside
        $(document).on('click', function (e) {
            if (!$(e.target).closest('.navbar').length && $('.navbar-collapse').hasClass('show')) {
                $('.navbar-collapse').collapse('hide');
            }
        });
    }

    // ========== SHARE FUNCTIONALITY ==========
    function initWebShare() {
        // Add share buttons
        if (navigator.share) {
            const shareBtn = $('<button class="btn btn-primary mt-3 w-100"><i class="fa fa-share-alt"></i> Haberi Paylaş</button>');
            $('.bg-white.border.p-4').first().append(shareBtn);

            shareBtn.on('click', async function () {
                try {
                    await navigator.share({
                        title: document.title,
                        text: $('h1').first().text(),
                        url: window.location.href
                    });
                } catch (err) {
                    console.log('Share failed:', err);
                }
            });
        }
    }

    // ========== VIBRATION FEEDBACK ==========
    function vibrateOnAction() {
        if ('vibrate' in navigator) {
            $('.btn, .badge, .nav-link').on('click', function () {
                navigator.vibrate(10); // Short vibration
            });
        }
    }

    // ========== NETWORK STATUS ==========
    function monitorNetworkStatus() {
        function updateOnlineStatus() {
            const condition = navigator.onLine ? 'online' : 'offline';
            if (condition === 'offline') {
                const offlineMsg = $('<div class="alert alert-warning text-center" style="position:fixed;top:0;left:0;right:0;z-index:99999;margin:0;">İnternet bağlantınız kesildi</div>');
                $('body').prepend(offlineMsg);
            } else {
                $('.alert-warning').fadeOut(300, function () {
                    $(this).remove();
                });
            }
        }

        window.addEventListener('online', updateOnlineStatus);
        window.addEventListener('offline', updateOnlineStatus);
    }

    // ========== SAVE SCROLL POSITION ==========
    function saveScrollPosition() {
        $(window).on('beforeunload', function () {
            sessionStorage.setItem('scrollPos', $(window).scrollTop());
        });

        const savedScroll = sessionStorage.getItem('scrollPos');
        if (savedScroll) {
            $(window).scrollTop(parseInt(savedScroll));
            sessionStorage.removeItem('scrollPos');
        }
    }

    // ========== INFINITE SCROLL (Optional) ==========
    let isLoadingMore = false;
    let currentPage = 1;

    function initInfiniteScroll() {
        $(window).on('scroll', function () {
            if ($(window).scrollTop() + $(window).height() >= $(document).height() - 200) {
                if (!isLoadingMore) {
                    loadMoreNews();
                }
            }
        });
    }

    function loadMoreNews() {
        isLoadingMore = true;
        currentPage++;

        showLoading();

        // Simulate API call (replace with actual API call)
        setTimeout(() => {
            hideLoading();
            isLoadingMore = false;
            // Append new news items here
        }, 1000);
    }

    // ========== PERFORMANCE MONITORING ==========
    function logPerformance() {
        if ('performance' in window) {
            window.addEventListener('load', () => {
                setTimeout(() => {
                    const perfData = performance.getEntriesByType('navigation')[0];
                    console.log('Page Load Time:', perfData.loadEventEnd - perfData.fetchStart, 'ms');
                    console.log('DOM Content Loaded:', perfData.domContentLoadedEventEnd - perfData.fetchStart, 'ms');
                }, 0);
            });
        }
    }

    // ========== ADD CSS FOR TOUCH FEEDBACK ==========
    const touchStyles = `
        <style>
        .touch-active {
            opacity: 0.7;
            transform: scale(0.98);
            transition: all 0.1s;
        }
        .nav-up {
            transform: translateY(-100%);
            transition: transform 0.3s;
        }
        .nav-down {
            transform: translateY(0);
            transition: transform 0.3s;
        }
        img.loaded {
            animation: fadeIn 0.3s;
        }
        @keyframes fadeIn {
            from { opacity: 0; }
            to { opacity: 1; }
        }
        </style>
    `;
    $('head').append(touchStyles);

    // ========== INITIALIZE ALL MOBILE FEATURES ==========
    $(document).ready(function () {
        if (isMobile) {
            console.log('Mobile optimizations enabled');

            // Initialize features
            optimizeImages();
            lazyLoadImages();
            improveFormInputs();
            improveTouchTargets();
            preventDoubleTapZoom();
            smoothScroll();
            autoCloseMenu();
            initWebShare();
            vibrateOnAction();
            monitorNetworkStatus();
            saveScrollPosition();
            initPullToRefresh();
            logPerformance();

            // Smart navbar (optional)
            // $(window).scroll(smartNavbar);

            // Infinite scroll (optional - uncomment to enable)
            // initInfiniteScroll();

            // Add mobile class to body
            $('body').addClass('mobile-optimized');

            // Log device info
            console.log('Device:', isIOS ? 'iOS' : isAndroid ? 'Android' : 'Mobile');
            console.log('Screen:', window.screen.width + 'x' + window.screen.height);
            console.log('Viewport:', window.innerWidth + 'x' + window.innerHeight);
        }
    });

    // ========== EXPOSE API FOR EXTERNAL USE ==========
    window.MobileOptimization = {
        showLoading: showLoading,
        hideLoading: hideLoading,
        vibrateDevice: () => navigator.vibrate && navigator.vibrate([50, 100, 50]),
        isMobile: isMobile,
        isIOS: isIOS,
        isAndroid: isAndroid
    };

})(jQuery);

// Dashboard Charts - Gercek verili grafikler
(function () {
    // Icerik Dagilimi - Doughnut Chart
    var contentCanvas = document.getElementById("contentDistribution");
    if (contentCanvas) {
        var haberler = parseInt(contentCanvas.dataset.haberler) || 0;
        var bloglar = parseInt(contentCanvas.dataset.bloglar) || 0;
        var slaytlar = parseInt(contentCanvas.dataset.slaytlar) || 0;
        var kategoriler = parseInt(contentCanvas.dataset.kategoriler) || 0;

        new Chart(contentCanvas, {
            type: 'doughnut',
            data: {
                labels: ['Haberler', 'Bloglar', 'Slaytlar', 'Kategoriler'],
                datasets: [{
                    data: [haberler, bloglar, slaytlar, kategoriler],
                    backgroundColor: [
                        '#667eea',
                        '#764ba2',
                        '#06B6D4',
                        '#8B5CF6'
                    ],
                    borderColor: '#fff',
                    borderWidth: 2,
                    hoverBorderWidth: 3,
                    hoverOffset: 6
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 1500,
                    easing: 'easeOutQuart'
                },
                plugins: {
                    legend: {
                        display: true,
                        position: 'bottom',
                        labels: {
                            padding: 20,
                            usePointStyle: true,
                            pointStyle: 'circle',
                            font: {
                                size: 12,
                                family: "'Lato', sans-serif"
                            }
                        }
                    }
                },
                cutout: '60%'
            }
        });
    }

    // Yorum Durumu - Bar Chart
    var commentCanvas = document.getElementById("commentStatus");
    if (commentCanvas) {
        var onaylananHaber = parseInt(commentCanvas.dataset.onaylananHaber) || 0;
        var bekleyenHaber = parseInt(commentCanvas.dataset.bekleyenHaber) || 0;
        var onaylananBlog = parseInt(commentCanvas.dataset.onaylananBlog) || 0;
        var bekleyenBlog = parseInt(commentCanvas.dataset.bekleyenBlog) || 0;

        new Chart(commentCanvas, {
            type: 'bar',
            data: {
                labels: ['Haber Yorumlari', 'Blog Yorumlari'],
                datasets: [
                    {
                        label: 'Onaylanan',
                        data: [onaylananHaber, onaylananBlog],
                        backgroundColor: 'rgba(34, 197, 94, 0.7)',
                        borderColor: '#16a34a',
                        borderWidth: 1,
                        borderRadius: 4
                    },
                    {
                        label: 'Bekleyen',
                        data: [bekleyenHaber, bekleyenBlog],
                        backgroundColor: 'rgba(245, 158, 11, 0.7)',
                        borderColor: '#d97706',
                        borderWidth: 1,
                        borderRadius: 4
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 1500,
                    easing: 'easeOutQuart'
                },
                plugins: {
                    legend: {
                        display: true,
                        position: 'bottom',
                        labels: {
                            padding: 20,
                            usePointStyle: true,
                            pointStyle: 'circle',
                            font: {
                                size: 12,
                                family: "'Lato', sans-serif"
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            stepSize: 1,
                            font: {
                                family: "'Lato', sans-serif"
                            }
                        },
                        grid: {
                            color: 'rgba(0, 0, 0, 0.05)'
                        }
                    },
                    x: {
                        ticks: {
                            font: {
                                family: "'Lato', sans-serif"
                            }
                        },
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });
    }
})();

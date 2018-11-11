
$(function () {
    var scrollTop = -1; // 鼠标进入到区域后，则存储当前window滚动条的高度
    $('.category-child-all').hover(function(){
        scrollTop = $(window).scrollTop();
    }, function(){
        scrollTop = -1;
    });

    // 鼠标进入到区域后，则强制window滚动条的高度
    $(window).scroll(function(){
        scrollTop!==-1 && $ (this).scrollTop(scrollTop);
    })
});

var blackcandy = (function () {

    var pagMoreFlag = 1;

    var init = function () {
        handleTag();
        handleShot();
        scroll2Top();
        handlePagination();
        handleSearch();
        showDescendantMenu();
        showVendor();
        showSidebar();
        showSupport();
        showWechat();
        handleImgBox();
        handleVideo();
        setLayoutType();
        handleCategoryChild();
    };
    var handleCategoryChild = function () {
        $(".btn-category-child").click(function () {
            $(".category-child-all").toggleClass("hidden");
            $(this).find("div").toggleClass("rotate-180");
        });
    };
    var handleTag = function () {
        $(".category-nav li a").click(function () {
            document.cookie = "tag=" + $(this).data("type") + ";path=/";
        });
    };
    var handleShot = function () {
        var shotUl = $(".shot-type");
        shotUl.find("li").click(function () {
            document.cookie = "shot=" + $(this).data("type") + ";path=/";
            window.location.reload(true);
        });
    };
    var scroll2Top = function () {
        $('.scrollTop').click(function(){
            $('body,html').animate({scrollTop:0},600);
            return false;
        });
    };
    var handlePagination = function () {
        $(window).scroll(function() {
            if (pagType == "infinite" && $(document).scrollTop() + $(window).height() > $(document).height() - 80 && pagMoreFlag == 1) {
                getMoreByPagination($(".pagination .more a"));
            }
        });
        $(".pagination .more a").click(function(){
            getMoreByPagination($(this));
            return false;
        });
    };

    var getMoreByPagination = function (_this) {
        pagMoreFlag = 0;
        _this.text("加载中...");
        $.ajax({
            type: "POST",
            url: _this.attr("href"),
            contentType: "application/x-www-form-urlencoded",
            success: function(data){
                result = $(data).find(".post-wrap").children();
                nextHref = $(data).find(".pagination .more a").attr("href");
                // ajax content fadeIn
                $(".post-wrap").append(result.fadeIn(500));
                pagMoreFlag = 1;
                _this.text("加载更多");
                if ( nextHref != undefined ) {
                    _this.attr("href", nextHref);
                } else {
                    // without more articles, remove the pagination navigatino
                    _this.remove();
                    pagMoreFlag = 0;
                    $(".pagination .more").append("没有更多文章了");
                }
            }
        });
    };

    var handleSearch = function () {
        var searchWrap = $(".search-wrap");
        var searchBtn = $(".search-button");
        $(document).on("click", ".search-button, .backdrop", function () {
            searchWrap.toggleClass("search-wrap-animate");
            searchBtn.find("i.czs-search-l").toggleClass("hidden");
            searchBtn.find("i.czs-close-l").parent().toggleClass("opacity-0 rotate-90");
            $(".backdrop").toggleClass("backdrop-animate");
        });
    };
    var showDescendantMenu = function () {
        // show sub/child/grand menu
        $(".menu-wrap .menu-item-has-children").click(function (e) {
            e.stopPropagation();
            if ($(this).hasClass("active")) {
                $(this).children(".sub-menu").slideUp(300);
                $(this).removeClass("active");
            } else {
                $(this).addClass("active");
                $(this).children(".sub-menu").slideDown(300);
            }
        });

        $('.header-menu .menu-item-has-children').hover(function () {
            $(this).children("a").toggleClass("active");
            $(this).children('.sub-menu').toggleClass("sub-menu-animate");
        });
    };
    var showVendor = function () {
        var vendorDom = $(".vendor");
        $(".btn-vendor").click(function () {
            vendorDom.toggleClass("hidden");
        });
    };

    var showSidebar = function () {
        $(".menu-button").click(function(e){
            if ($(this).find(".nav-bar").hasClass("nav-bar-animate")) {
                $(this).find(".nav-bar").removeClass("nav-bar-animate");
                $(".menu-wrap").removeClass("open-menu-wrap");
                $(".menu-wrap-backdrop").animate({
                    opacity: 0
                }, 430, function(){
                    $(this).remove();
                });
            } else {
                $(this).find(".nav-bar").addClass("nav-bar-animate");
                $(".menu-wrap").addClass("open-menu-wrap");
                $(this).append("<div class='menu-wrap-backdrop fixed-fluid'></div>")
            }
        });
    };

    var showSupport = function () {
        var supportImg = $(".article-support-img");
        var windowW = $(window).width();
        if (windowW <= 767) {
            supportImg.width(windowW - 30);
        }
        $('.article-support-button .btn').bind({
            mouseenter: function(){
                supportImg.show();
                supportImg.animate({
                    bottom: '46px',
                    opacity: 1
                }, 300);
            }, mouseleave: function() {
                supportImg.animate({
                    bottom: '58px',
                    opacity: 0
                }, 300, function(){
                    $(this).hide();
                });
            }
        });
    };

    var showWechat = function () {
        $(".follow-wechat").bind({
            mouseenter: function () {
                $(this).find('.follow-wechat-popup').show();
                $(this).find('.follow-wechat-popup').animate({
                    bottom: '48px',
                    opacity: 1
                }, 300);
            }, mouseleave: function () {
                $(this).find('.follow-wechat-popup').animate({
                    bottom: '58px',
                    opacity: 0
                }, 300, function () {
                    $(this).hide();
                });
            }
        });
    };

    var handleImgBox = function () {
        if (fancyboxSwitcher && !isHomePage) {
            var siteTitle = $("title").html();
            $(".article-body img, .page-common img").not(".attachment-thumbnail").each(function () {
                var alt = this.alt;
                var src = $(this).attr("src");
                if (!alt) {
                    $(this).attr("alt", siteTitle);
                }
                $(this).wrap("<a href='"+ src +"' class='fancybox' rel='fancybox-group' title='"+ alt +"'></a>");
            });

            $(".fancybox").fancybox({
                'padding': 0,
                'opacity': true,
                'cyclic': true
            });
        }
    };
    var handleVideo = function () {
        if ($(window).width() < 576) {
            $(".article-body iframe").each(function () {
                $(this).height(200);
            });
        }
    };
    var setLayoutType = function () {
        $(".layout-type-wrap").hover(function () {
            $(this).toggleClass("layout-type-wrap-animate");
        });
        $(".layout-type li").click(function () {
            document.cookie = "layout=" + $(this).data("type");
            window.location.reload(true);
        });
    };
    return {
        init: init
    }
})();
blackcandy.init();

var mainWidth = $('#main').innerWidth();
window.onload = function () {
    $(window).trigger("scroll");
};
$(window).resize(function () {
    mainWidth = $('#main').innerWidth();
    $(window).trigger("scroll");
    handleCarousel();
});

handleCarousel();
function handleCarousel() {
    if (carouselSwitcher && isHomePage) {
        var carouselDom = $("#carousel");
        //$(".carousel-overlay").css("opacity", parseInt(carouselOpacity) / 100);
        carouselDom.owlCarousel({
            items: 1,
            loop: true,
            autoplay: true,
            autoplayHoverPause: true,
            nav : true,
            navText:'',
            animateOut: "fadeOut",
            animateIn: "fadeIn"
        });

        var carouselNavPrevDom = carouselDom.find(".owl-prev");
        var carouselNavNextDom = carouselDom.find(".owl-next");
        if (carouselNavPrevDom.find("i").length == 0) {
            carouselNavPrevDom.addClass("transition").append('<i class="czs-angle-left-l">');
            carouselNavNextDom.addClass("transition").append('<i class="czs-angle-right-l">');
        }
        carouselDom.hover(function () {
            carouselNavPrevDom.toggleClass("owl-prev-animation");
            carouselNavNextDom.toggleClass("owl-next-animation");
            $(this).toggleClass("hover");
        });

    }
}

if (carouselSwitcher && carouselMouseSwitcher) {
    var carouselDom = $("#carousel");
    carouselDom.on('mousewheel', '.owl-stage', function (e) {
        if (e.deltaY>0) {
            carouselDom.trigger('next.owl');
        } else {
            carouselDom.trigger('prev.owl');
        }
        e.preventDefault();
    });
}

// sidebar fixed
var sidebarDom = $('#sidebar');
var affixDom = $(".affix");
var headerH = $('.header').height();
var scrollTop = 0;
var sidebarH = sidebarDom.outerHeight();
var sidebar2Top = sidebarDom.length >= 1 && sidebarDom.offset().top;
var bodyH = $(document).height();

$(window).scroll(function() {
    var sidebarW = sidebarDom.width();
    var footerH = $('#footer').innerHeight();
    var windowH = $(window).height();
    var affixH = affixDom.innerHeight();
    var leftH = (windowH - headerH - affixH - 6) > 0 ? (windowH - headerH - affixH - 6) : 0;
    bodyH = $(document).height();
    scrollTop = $(document).scrollTop();
    if (scrollTop > 1200) {
        $('.scrollTop').addClass("active");
    } else {
        $('.scrollTop').removeClass("active");
    }
    var scrollBottom = bodyH - windowH - scrollTop;
    if (scrollTop > sidebar2Top + sidebarH) {
        if (scrollTop < (bodyH - footerH - windowH + leftH)) {
            affixDom.css({
                position: 'fixed',
                top: '88px',
                bottom: '',
                width: sidebarW + 'px'
            });
        } else {
            affixDom.css({
                position: 'fixed',
                top: '',
                bottom: footerH - scrollBottom,
                width: sidebarW + 'px'
            });
        }
    } else {
        affixDom.css({
            position: '',
            width: sidebarW + 'px'
        });
    }
});
/**
 *
 * @returns {boolean}
 */
$.fn.postLike = function() {
    var id = $(this).data("id");
    var action = $(this).data('action');
    var rateHolder = $(this).find(".count");
    var ajaxData = {
        um_id: id,
        um_action: action
    };
    var ajaxContent = {
        type: "POST",
        url: siteUrl+"/wp-admin/admin-ajax.php",
        data: ajaxData
    };
    var _this = $(this);
    if ($(this).hasClass('done')) {
        $(this).removeClass('done');
        ajaxData.action = "subLike";
        ajaxContent.success = function (data) {
            _this.find("i").removeClass("czs-thumbs-up").addClass("czs-thumbs-up-l");
            $(rateHolder).html(data);
        };
        $.ajax(ajaxContent);
    } else {
        $(this).addClass('done');
        ajaxData.action = "addLike";
        ajaxContent.success = function (data) {
            _this.find('i').removeClass("czs-thumbs-up-l").addClass("czs-thumbs-up");
            $(rateHolder).html(data);
        };
        $.ajax(ajaxContent);
    }
    return false;
};
$(document).on("click", ".favorite", function() {
        $(this).postLike();
});


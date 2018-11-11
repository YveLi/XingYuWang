/*
 * main.js
 * 日期: 2018.2.27
 * By Kehen
*/
$(function () {
    initPage();
});

function initPage() {
    //导航
    layui.use('element', function () {
        var element = layui.element;
    });

    //幻灯
    layui.use('carousel', function () {
        var index_banner = layui.carousel;
        index_banner.render({
            elem: '#index_banner'
            , width: '100%' //设置容器宽度
            , height: '100%'
            , index: 0
            , indicator: 'none'
            , interval: 7000
        });

        var index_news_banner = layui.carousel;
        index_news_banner.render({
            elem: '#index_news_banner'
            , width: '100%' //设置容器宽度
            , height: '100%'
            , anim: 'fade'
            , index: 0
            , interval: 5000
            , indicator: 'none'
        });

        var news_banner = layui.carousel;
        news_banner.render({
            elem: '#news_banner'
            , width: '100%'
            , height: '100%'
            , interval: 5000
            , arrow: 'always'
            , anim: 'updown'
            , indicator: 'none'
        });

        var auchor_img = layui.carousel;
        auchor_img.render({
            elem: '#auchor_img'
            , width: '100%'
            , height: '100%'
            , autoplay: false
            , interval: 3000
            , arrow: 'none'
            , indicator: 'inside'
        });

    });

    //表单
    layui.use('form', function () {
        var form = layui.form;

        //开关
        form.on('switch(switchAuchor)', function (data) {
            if (data.elem.checked) {
                $('.add-dis').addClass('show');
            } else {
                $('.add-dis').removeClass('show');
            }
        });

    });


    layui.use('layedit', function () {
        var layedit = layui.layedit;
        var anchor_con = layedit.build('auchor_editor', {
            height: 300
            , tool: [
                'strong'
                , 'italic'
                , 'underline'
                , 'del'
                , '|'
                , 'left'
                , 'center'
                , 'right'
            ]
        });

        $('#add_anchor').on('click', function () {
            layedit.sync(anchor_con);
        });
    });

    layui.use('util', function () {
        var util = layui.util
        util.fixbar({
            bar1: true,
            showHeight: '500',
            css: { right: 10, bottom: 100 },
            bgcolor: '#393939',
            click: function (type) {
                if (type === 'bar1') {
                    layer.msg('点击了bar1');
                }
            }
        });
    });


    layui.use('layer', function () {
        var layer = layui.layer;
        var w_code = $('.weixin-code').html();
        var w_name = $('.weixin-code img').attr('alt');

        $('ul.author-link .weixin').on('click', function () {
            layer.open({
                content: w_code
                , title: w_name + '的微信公众号'
                , skin: 'w-code'
                , shadeClose: true
                , btn: ''
            });
        });

        $('div.anchor-contect-con .layui-timeline .anchor-remind').on('click', function () {
            var a_code = $(this).parent().find('.weixin-code').html();
            var a_name = $(this).parent().find('.weixin-code img').attr('alt');

            layer.open({
                content: a_code
                , title: a_name
                , skin: 'w-code'
                , shadeClose: true
                , btn: ''
            });
        });

        $('.zb-btn-remind').on('click', function () {
            var a_code = $('.anchor-remind-special').find('.weixin-code').html();
            var a_name = $('.anchor-remind-special').find('.weixin-code img').attr('alt');

            layer.open({
                content: a_code
                , title: a_name
                , skin: 'w-code'
                , shadeClose: true
                , btn: ''
            });
        });


        $('div.page-nav .web-link').on('click', function () {
            var url_link = $(this).data("link");
            var url_name = $(this).find('.name').html();

            var url_index = layer.open({
                title: '前往' + url_name
                , content: '传送门开启中...'
                , shadeClose: true
                , skin: 'web-link-url'
                , btn: ['立即前往']
                , btnAlign: 'c'
                , success: function (layero, index) {
                    console.log(layero);
                    layero.find('.layui-layer-btn0').attr({
                        'href': url_link
                        , 'target': "_blank"
                    });
                }
            });

            $('.web-link-url .layui-layer-btn0').on('click', function () {
                layer.close(url_index);
            });
        });
    });
}

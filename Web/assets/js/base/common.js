function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function menuorder(ordid){
    $(".user-nav-list li").eq(ordid).addClass("active");
}

$(function () {    
    $(".menubox").mouseenter(function () {
        $(".menulist").show();
    });
    $(".menubox").mouseleave(function () {
        $(".menulist").hide();
    });
  
    if (!$("#userloginname").data("login")) {
    }

    $(".menulist li[data-type=3],.loginouts").click(function () {
        layer.confirm('您确认退出系统吗？', {
            btn: ['确定', '取消'] //按钮
        }, function () {
            $.post("/index/Logout", function (data) {
                if (data == "ok") {
                    $("#userloginname").text("我的账户").attr("title", "请先登录账户").data("login", "0");
                    location.href = '/index/home'
                }
            })
        }, function () {
             
        });
    });
    $(".menulist li[data-type=1]").click(function () {
        if ($("#userloginname").data("login") == "0") return;      
        location.href = '/usercenter/archive';
    });

    $(".menu a").click(function () {
        var flag = $(this).parent().data("ord");
        if (flag != 0) {
            if (!$("#userloginname").data("login")) {

                if ($(".userlogin")) {
                    $(".userlogin").first().animate({ "right": 100 }, 500, function () {
                        $(".userlogin").first().animate({ "right": 0 }, 500, "easeOutElastic", function () {
                            $(".logintip").html("提示：请您先登录账号").show();
                        });
                    });
                }
                return false;
            }
        }
    });
});


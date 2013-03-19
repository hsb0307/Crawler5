function getHtml(dom) {
    var log = document.getElementById("TraceConsole");
    if (typeof (dom) === "undefined" || dom === null) {
        log.value = document.documentElement.outerHTML;
    }
    else {
        log.value = document.getElementById(dom).outerHTML;
    }
    log.parentNode.style.display = "block";
}


function newGuid() {
    var S4 = function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);

    };
    //return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
    return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
}

function createFrame(url, id) {
    var frameId;
    if (typeof (id) === "undefined" || id === null) {
        frameId = newGuid();
    }
    else {
        frameId = id;
    }
    var s = '<iframe id="iframe' + frameId + '" name="mainFrame' + frameId + '" scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:98%;"></iframe>';
    return s;
}
////var tabs = $('#tabControl')
function addTab(title, url) {
    if ($('#tabControl').tabs('exists', title)) {
        $('#tabControl').tabs('select', title);
    } else {
        $('#tabControl').tabs('add', { // $('#tabControl').
            title: title,
            content: createFrame(url),
            closable: true
        });
    }
}

function addTabToParent(title, url) {
    if (parent.$('#tabControl').tabs('exists', title)) {
        parent.$('#tabControl').tabs('select', title);
    } else {
        parent.$('#tabControl').tabs('add', { // $('#tabControl').
            title: title,
            content: createFrame(url),
            closable: true
        });
    }
}

function getFormatDate(date, dateformat) {
    if (isNaN(date)) return null;
    var format = dateformat;
    var o = {
        "M+": date.getMonth() + 1,
        "d+": date.getDate(),
        "h+": date.getHours(),
        "m+": date.getMinutes(),
        "s+": date.getSeconds(),
        "q+": Math.floor((date.getMonth() + 3) / 3),
        "S": date.getMilliseconds()
    }
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (date.getFullYear() + "")
    .substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k]
        : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

function convertDateString(value) {
    if (typeof (value) == "string" && /^\/Date/.test(value)) {
        value = value.replace(/^\//, "new ").replace(/\/$/, "");
        eval("value = " + value);
        if (value instanceof Date) {
            var format = "yyyy-MM-dd hh:mm:ss";
            return getFormatDate(value, format);

        }
        else {
            return value.toString();
        }
    }
    return value;
}

$('body').append('<div id="popWindow" class="easyui-dialog" closed="true"></div>');

function showWindow(title, url, width, height, modal, minimizable, maximizable) {

    $('#popWindow').window({
        title: title,
        width: width === undefined ? 600 : width,
        height: height === undefined ? 400 : height,
        //content: '<iframe scrolling="yes" frameborder="0"  src="' + href + '" style="width:100%;height:98%;"></iframe>',
        //        href: href === undefined ? null : href,
        content: createFrame(url),
        modal: modal === undefined ? true : modal,
        minimizable: minimizable === undefined ? false : minimizable,
        maximizable: maximizable === undefined ? false : maximizable,
        shadow: false,
        cache: false,
        closed: false,
        collapsible: false,
        resizable: false,
        loadingMessage: '正在加载数据，请稍等片刻......'
    });

};

$(function ($) {
    //alert("load showwin");
});

/*!
 * jQuery Tools v1.2.7 - The missing UI library for the Web
 * 
 * overlay/overlay.js
 * 
 * NO COPYRIGHTS OR LICENSES. DO WHAT YOU LIKE.
 * 
 * http://flowplayer.org/tools/
 * 
 */
(function (a) { a.tools = a.tools || { version: "v1.2.7" }, a.tools.overlay = { addEffect: function (a, b, d) { c[a] = [b, d] }, conf: { close: null, closeOnClick: !0, closeOnEsc: !0, closeSpeed: "fast", effect: "default", fixed: !a.browser.msie || a.browser.version > 6, left: "center", load: !1, mask: null, oneInstance: !0, speed: "normal", target: null, top: "10%" } }; var b = [], c = {}; a.tools.overlay.addEffect("default", function (b, c) { var d = this.getConf(), e = a(window); d.fixed || (b.top += e.scrollTop(), b.left += e.scrollLeft()), b.position = d.fixed ? "fixed" : "absolute", this.getOverlay().css(b).fadeIn(d.speed, c) }, function (a) { this.getOverlay().fadeOut(this.getConf().closeSpeed, a) }); function d(d, e) { var f = this, g = d.add(f), h = a(window), i, j, k, l = a.tools.expose && (e.mask || e.expose), m = Math.random().toString().slice(10); l && (typeof l == "string" && (l = { color: l }), l.closeOnClick = l.closeOnEsc = !1); var n = e.target || d.attr("rel"); j = n ? a(n) : null || d; if (!j.length) throw "Could not find Overlay: " + n; d && d.index(j) == -1 && d.click(function (a) { f.load(a); return a.preventDefault() }), a.extend(f, { load: function (d) { if (f.isOpened()) return f; var i = c[e.effect]; if (!i) throw "Overlay: cannot find effect : \"" + e.effect + "\""; e.oneInstance && a.each(b, function () { this.close(d) }), d = d || a.Event(), d.type = "onBeforeLoad", g.trigger(d); if (d.isDefaultPrevented()) return f; k = !0, l && a(j).expose(l); var n = e.top, o = e.left, p = j.outerWidth({ margin: !0 }), q = j.outerHeight({ margin: !0 }); typeof n == "string" && (n = n == "center" ? Math.max((h.height() - q) / 2, 0) : parseInt(n, 10) / 100 * h.height()), o == "center" && (o = Math.max((h.width() - p) / 2, 0)), i[0].call(f, { top: n, left: o }, function () { k && (d.type = "onLoad", g.trigger(d)) }), l && e.closeOnClick && a.mask.getMask().one("click", f.close), e.closeOnClick && a(document).on("click." + m, function (b) { a(b.target).parents(j).length || f.close(b) }), e.closeOnEsc && a(document).on("keydown." + m, function (a) { a.keyCode == 27 && f.close(a) }); return f }, close: function (b) { if (!f.isOpened()) return f; b = b || a.Event(), b.type = "onBeforeClose", g.trigger(b); if (!b.isDefaultPrevented()) { k = !1, c[e.effect][1].call(f, function () { b.type = "onClose", g.trigger(b) }), a(document).off("click." + m + " keydown." + m), l && a.mask.close(); return f } }, getOverlay: function () { return j }, getTrigger: function () { return d }, getClosers: function () { return i }, isOpened: function () { return k }, getConf: function () { return e } }), a.each("onBeforeLoad,onStart,onLoad,onBeforeClose,onClose".split(","), function (b, c) { a.isFunction(e[c]) && a(f).on(c, e[c]), f[c] = function (b) { b && a(f).on(c, b); return f } }), i = j.find(e.close || ".close"), !i.length && !e.close && (i = a("<a class=\"close\"></a>"), j.prepend(i)), i.click(function (a) { f.close(a) }), e.load && f.load() } a.fn.overlay = function (c) { var e = this.data("overlay"); if (e) return e; a.isFunction(c) && (c = { onBeforeLoad: c }), c = a.extend(!0, {}, a.tools.overlay.conf, c), this.each(function () { e = new d(a(this), c), b.push(e), a(this).data("overlay", e) }); return c.api ? e : this } })(jQuery);

/**
 * base64 encoding & decoding
 * for fixing browsers which don't support Base64 | btoa |atob
 */

(function (win, undefined) {

    var Base64 = function () {
        var base64hash = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/';

        // btoa method
        function _btoa(s) {
            if (/([^\u0000-\u00ff])/.test(s)) {
                throw new Error('INVALID_CHARACTER_ERR');
            }
            var i = 0,
				prev,
				ascii,
				mod,
				result = [];

            while (i < s.length) {
                ascii = s.charCodeAt(i);
                mod = i % 3;

                switch (mod) {
                    // 第一个6位只需要让8位二进制右移两位
                    case 0:
                        result.push(base64hash.charAt(ascii >> 2));
                        break;
                        //第二个6位 = 第一个8位的后两位 + 第二个8位的前4位
                    case 1:
                        result.push(base64hash.charAt((prev & 3) << 4 | (ascii >> 4)));
                        break;
                        //第三个6位 = 第二个8位的后4位 + 第三个8位的前2位
                        //第4个6位 = 第三个8位的后6位
                    case 2:
                        result.push(base64hash.charAt((prev & 0x0f) << 2 | (ascii >> 6)));
                        result.push(base64hash.charAt(ascii & 0x3f));
                        break;
                }

                prev = ascii;
                i++;
            }

            // 循环结束后看mod, 为0 证明需补3个6位，第一个为最后一个8位的最后两位后面补4个0。另外两个6位对应的是异常的“=”；
            // mod为1，证明还需补两个6位，一个是最后一个8位的后4位补两个0，另一个对应异常的“=”
            if (mod == 0) {
                result.push(base64hash.charAt((prev & 3) << 4));
                result.push('==');
            } else if (mod == 1) {
                result.push(base64hash.charAt((prev & 0x0f) << 2));
                result.push('=');
            }

            return result.join('');
        }

        // atob method
        // 逆转encode的思路即可
        function _atob(s) {
            s = s.replace(/\s|=/g, '');
            var cur,
				prev,
				mod,
				i = 0,
				result = [];

            while (i < s.length) {
                cur = base64hash.indexOf(s.charAt(i));
                mod = i % 4;

                switch (mod) {
                    case 0:
                        //TODO
                        break;
                    case 1:
                        result.push(String.fromCharCode(prev << 2 | cur >> 4));
                        break;
                    case 2:
                        result.push(String.fromCharCode((prev & 0x0f) << 4 | cur >> 2));
                        break;
                    case 3:
                        result.push(String.fromCharCode((prev & 3) << 6 | cur));
                        break;

                }

                prev = cur;
                i++;
            }

            return result.join('');
        }

        return {
            btoa: _btoa,
            atob: _atob,
            encode: _btoa,
            decode: _atob
        };
    }();

    if (!win.Base64) { win.Base64 = Base64 }
    if (!win.btoa) { win.btoa = Base64.btoa }
    if (!win.atob) { win.atob = Base64.atob }

})(window)
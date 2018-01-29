using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GibsonWeds.Utility;

namespace GibsonWeds.Utility
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString TimeDropDownList(this HtmlHelper html, string name, object htmlAttributes, int selectedHr = 0)
        {
            var list = new SelectList(new[]
            {
                new { Key = "0", Value = "00:00" },
                new { Key = "1", Value = "01:00" },
                new { Key = "2", Value = "02:00" },
                new { Key = "3", Value = "03:00" },
                new { Key = "4", Value = "04:00" },
                new { Key = "5", Value = "05:00" },
                new { Key = "6", Value = "06:00" },
                new { Key = "7", Value = "07:00" },
                new { Key = "8", Value = "08:00" },
                new { Key = "9", Value = "09:00" },
                new { Key = "10", Value = "10:00" },
                new { Key = "11", Value = "11:00" },
                new { Key = "12", Value = "12:00" },
                new { Key = "13", Value = "13:00" },
                new { Key = "14", Value = "14:00" },
                new { Key = "15", Value = "15:00" },
                new { Key = "16", Value = "16:00" },
                new { Key = "17", Value = "17:00" },
                new { Key = "18", Value = "18:00" },
                new { Key = "19", Value = "19:00" },
                new { Key = "20", Value = "20:00" },
                new { Key = "21", Value = "21:00" },
                new { Key = "22", Value = "22:00" },
                new { Key = "23", Value = "23:00" },
                new { Key = "24", Value = "24:00" },
            }, "Key", "Value", selectedHr);
            return SelectExtensions.DropDownList(html, name, list, htmlAttributes);
        }

        public static MvcHtmlString IconDropDownList(this HtmlHelper html, string name, object htmlAttributes, int selectedIcon = 0)
        {
            string[] arr = new string[] { "fa-adjust", "fa-adn", "fa-align-center", "fa-align-justify", "fa-align-left", "fa-align-right", "fa-ambulance", "fa-anchor", "fa-android", "fa-angle-double-down", "fa-angle-double-left", "fa-angle-double-right", "fa-angle-double-up", "fa-angle-down", "fa-angle-left", "fa-angle-right", "fa-angle-up", "fa-apple", "fa-archive", "fa-arrow-circle-down", "fa-arrow-circle-left", "fa-arrow-circle-o-down", "fa-arrow-circle-o-left", "fa-arrow-circle-o-right", "fa-arrow-circle-o-up", "fa-arrow-circle-right", "fa-arrow-circle-up", "fa-arrow-down", "fa-arrow-left", "fa-arrow-right", "fa-arrow-up", "fa-arrows", "fa-arrows-alt", "fa-arrows-h", "fa-arrows-v", "fa-asterisk", "fa-backward", "fa-ban", "fa-bar-chart-o", "fa-barcode", "fa-bars", "fa-beer", "fa-bell", "fa-bell-o", "fa-bitbucket", "fa-bitbucket-square", "fa-bitcoin", "fa-bold", "fa-bolt", "fa-book", "fa-bookmark", "fa-bookmark-o", "fa-briefcase", "fa-btc", "fa-bug", "fa-building-o", "fa-bullhorn", "fa-bullseye", "fa-calendar", "fa-calendar-o", "fa-camera", "fa-camera-retro", "fa-caret-down", "fa-caret-left", "fa-caret-right", "fa-caret-square-o-down", "fa-caret-square-o-left", "fa-caret-square-o-right", "fa-caret-square-o-up", "fa-caret-up", "fa-certificate", "fa-chain", "fa-chain-broken", "fa-check", "fa-check-circle", "fa-check-circle-o", "fa-check-square", "fa-check-square-o", "fa-chevron-circle-down", "fa-chevron-circle-left", "fa-chevron-circle-right", "fa-chevron-circle-up", "fa-chevron-down", "fa-chevron-left", "fa-chevron-right", "fa-chevron-up", "fa-circle", "fa-circle-o", "fa-clipboard", "fa-clock-o", "fa-cloud", "fa-cloud-download", "fa-cloud-upload", "fa-cny", "fa-code", "fa-code-fork", "fa-coffee", "fa-cog", "fa-cogs", "fa-columns", "fa-comment", "fa-comment-o", "fa-comments", "fa-comments-o", "fa-compass", "fa-compress", "fa-copy", "fa-credit-card", "fa-crop", "fa-crosshairs", "fa-css3", "fa-cut", "fa-cutlery", "fa-dashboard", "fa-dedent", "fa-desktop", "fa-dollar", "fa-dot-circle-o", "fa-download", "fa-dribbble", "fa-dropbox", "fa-edit", "fa-eject", "fa-ellipsis-h", "fa-ellipsis-v", "fa-envelope", "fa-envelope-o", "fa-eraser", "fa-eur", "fa-euro", "fa-exchange", "fa-exclamation", "fa-exclamation-circle", "fa-exclamation-triangle", "fa-expand", "fa-external-link", "fa-external-link-square", "fa-eye", "fa-eye-slash", "fa-facebook", "fa-facebook-square", "fa-fast-backward", "fa-fast-forward", "fa-female", "fa-fighter-jet", "fa-file", "fa-file-o", "fa-file-text", "fa-file-text-o", "fa-files-o", "fa-film", "fa-filter", "fa-fire", "fa-fire-extinguisher", "fa-flag", "fa-flag-checkered", "fa-flag-o", "fa-flash", "fa-flask", "fa-flickr", "fa-floppy-o", "fa-folder", "fa-folder-o", "fa-folder-open", "fa-folder-open-o", "fa-font", "fa-forward", "fa-foursquare", "fa-frown-o", "fa-gamepad", "fa-gavel", "fa-gbp", "fa-gear", "fa-gears", "fa-gift", "fa-github", "fa-github-alt", "fa-github-square", "fa-gittip", "fa-glass", "fa-globe", "fa-google-plus", "fa-google-plus-square", "fa-group", "fa-h-square", "fa-hand-o-down", "fa-hand-o-left", "fa-hand-o-right", "fa-hand-o-up", "fa-hdd-o", "fa-headphones", "fa-heart", "fa-heart-o", "fa-home", "fa-hospital-o", "fa-html5", "fa-inbox", "fa-indent", "fa-info", "fa-info-circle", "fa-inr", "fa-instagram", "fa-italic", "fa-jpy", "fa-key", "fa-keyboard-o", "fa-krw", "fa-laptop", "fa-leaf", "fa-legal", "fa-lemon-o", "fa-level-down", "fa-level-up", "fa-lightbulb-o", "fa-link", "fa-linkedin", "fa-linkedin-square", "fa-linux", "fa-list", "fa-list-alt", "fa-list-ol", "fa-list-ul", "fa-location-arrow", "fa-lock", "fa-long-arrow-down", "fa-long-arrow-left", "fa-long-arrow-right", "fa-long-arrow-up", "fa-magic", "fa-magnet", "fa-mail-forward", "fa-mail-reply", "fa-mail-reply-all", "fa-male", "fa-map-marker", "fa-maxcdn", "fa-medkit", "fa-meh-o", "fa-microphone", "fa-microphone-slash", "fa-minus", "fa-minus-circle", "fa-minus-square", "fa-minus-square-o", "fa-mobile", "fa-mobile-phone", "fa-money", "fa-moon-o", "fa-music", "fa-outdent", "fa-pagelines", "fa-paperclip", "fa-paste", "fa-pause", "fa-pencil", "fa-pencil-square", "fa-pencil-square-o", "fa-phone", "fa-phone-square", "fa-picture-o", "fa-pinterest", "fa-pinterest-square", "fa-plane", "fa-play", "fa-play-circle", "fa-play-circle-o", "fa-plus", "fa-plus-circle", "fa-plus-square", "fa-plus-square-o", "fa-power-off", "fa-print", "fa-puzzle-piece", "fa-qrcode", "fa-question", "fa-question-circle", "fa-quote-left", "fa-quote-right", "fa-random", "fa-refresh", "fa-renren", "fa-repeat", "fa-reply", "fa-reply-all", "fa-retweet", "fa-rmb", "fa-road", "fa-rocket", "fa-rotate-left", "fa-rotate-right", "fa-rouble", "fa-rss", "fa-rss-square", "fa-rub", "fa-ruble", "fa-rupee", "fa-save", "fa-scissors", "fa-search", "fa-search-minus", "fa-search-plus", "fa-share", "fa-share-square", "fa-share-square-o", "fa-shield", "fa-shopping-cart", "fa-sign-in", "fa-sign-out", "fa-signal", "fa-sitemap", "fa-skype", "fa-smile-o", "fa-sort", "fa-sort-alpha-asc", "fa-sort-alpha-desc", "fa-sort-amount-asc", "fa-sort-amount-desc", "fa-sort-asc", "fa-sort-desc", "fa-sort-down", "fa-sort-numeric-asc", "fa-sort-numeric-desc", "fa-sort-up", "fa-spinner", "fa-square", "fa-square-o", "fa-stack-exchange", "fa-stack-overflow", "fa-star", "fa-star-half", "fa-star-half-empty", "fa-star-half-full", "fa-star-half-o", "fa-star-o", "fa-step-backward", "fa-step-forward", "fa-stethoscope", "fa-stop", "fa-strikethrough", "fa-subscript", "fa-suitcase", "fa-sun-o", "fa-superscript", "fa-table", "fa-tablet", "fa-tachometer", "fa-tag", "fa-tags", "fa-tasks", "fa-terminal", "fa-text-height", "fa-text-width", "fa-th", "fa-th-large", "fa-th-list", "fa-thumb-tack", "fa-thumbs-down", "fa-thumbs-o-down", "fa-thumbs-o-up", "fa-thumbs-up", "fa-ticket", "fa-times", "fa-times-circle", "fa-times-circle-o", "fa-tint", "fa-toggle-down", "fa-toggle-left", "fa-toggle-right", "fa-toggle-up", "fa-trash-o", "fa-trello", "fa-trophy", "fa-truck", "fa-try", "fa-tumblr", "fa-tumblr-square", "fa-turkish-lira", "fa-twitter", "fa-twitter-square", "fa-umbrella", "fa-underline", "fa-undo", "fa-unlink", "fa-unlock", "fa-unlock-alt", "fa-unsorted", "fa-upload", "fa-usd", "fa-user", "fa-user-md", "fa-users", "fa-video-camera", "fa-vimeo-square", "fa-vk", "fa-volume-down", "fa-volume-off", "fa-volume-up", "fa-warning", "fa-weibo", "fa-wheelchair", "fa-windows", "fa-won", "fa-wrench", "fa-xing", "fa-xing-square", "fa-yen", "fa-youtube", "fa-youtube-play" };

            var arrList = Enumerable.Repeat(new { Key = "", Value = "" }, 0).ToList();
            foreach (var item in arr)
            {
                arrList.Add(new { Key = "fa " + item, Value = item.Remove(0, 3) });
            }
            var list = new SelectList(arrList.ToArray(), "Key", "Value", selectedIcon);
            return SelectExtensions.DropDownList(html, name, list, "Select icon", htmlAttributes);
        }



        public static MvcHtmlString SiteModeIndicator(this HtmlHelper html)
        {
            var sitemode = Config.sitemode;

            if (sitemode == "local")
                return new MvcHtmlString("<div style='background: red; color: white; font-style: italic; text-align: center; position: absolute; width: 100%; z-index: 999;'><b>local test site</b> <a style='color: black; text-decoration: underline;cursor:pointer;' onclick='$(this).parent().hide();'>(hide)</a></div>");
            if (sitemode == "beta")
                return new MvcHtmlString("<div style='background: red; color: white; font-style: italic; text-align: center; position: absolute; width: 100%; z-index: 999;'><b>beta test site</b> <a style='color: black; text-decoration: underline;cursor:pointer;' onclick='$(this).parent().hide();'>(hide)</a></div>");
            if (sitemode != "live")
                return new MvcHtmlString("<div style='background: red; color: white; font-style: italic; text-align: center; position: absolute; width: 100%; z-index: 999;'><b>test site</b> <a style='color: black; text-decoration: underline;cursor:pointer;' onclick='$(this).parent().hide();'>(hide)</a></div>");

            return new MvcHtmlString("");
        }

        public static MvcHtmlString url_equals(this HtmlHelper html, string valueToReturn, string url)
        {
            if (url.Equals(html.ViewContext.HttpContext.Request.Url.AbsolutePath, StringComparison.InvariantCultureIgnoreCase))
                return new MvcHtmlString(valueToReturn);
            else
                return new MvcHtmlString("");
        }

        public static MvcHtmlString url_equals(this HtmlHelper html, string valueToReturn, params string[] urls)
        {
            foreach (var url in urls)
                if (url.Equals(html.ViewContext.HttpContext.Request.Url.AbsolutePath, StringComparison.InvariantCultureIgnoreCase))
                    return new MvcHtmlString(valueToReturn);

            return new MvcHtmlString("");
        }

        public static MvcHtmlString url_startswith(this HtmlHelper html, string valueToReturn, string url)
        {
            if (html.ViewContext.HttpContext.Request.Url.AbsolutePath.StartsWith(url, StringComparison.InvariantCultureIgnoreCase))
                return new MvcHtmlString(valueToReturn);
            else
                return new MvcHtmlString("");
        }

        public static MvcHtmlString url_startswith(this HtmlHelper html, string valueToReturn, params string[] urls)
        {
            foreach (var url in urls)
                if (html.ViewContext.HttpContext.Request.Url.AbsolutePath.StartsWith(url, StringComparison.InvariantCultureIgnoreCase))
                    return new MvcHtmlString(valueToReturn);

            return new MvcHtmlString("");
        }

        public static MvcHtmlString no_text_indicator(this HtmlHelper html, string value, string indicator = "...")
        {
            if (String.IsNullOrWhiteSpace(value))
                return new MvcHtmlString(indicator);
            else
                return new MvcHtmlString(value);
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DotNetBrowser.WinForms.Demo.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DotNetBrowser.WinForms.Demo.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to body {
        ///	font: 14px/1.72 &apos;Open Sans&apos;, Arial, sans-serif;
        ///}
        ///
        ///.default {
        ///	background-color: rgb(60, 60, 60);
        ///	color: white;
        ///}
        ///
        ///.main {
        ///	width: 75%;
        ///	height: 75%;
        ///	margin: auto;
        ///	padding:  15px 50px;
        ///}
        ///
        ///.popup {
        ///	border: 2px dashed #000000;
        ///	border-radius: 4px;
        ///	text-align: center;
        ///	font-weight: 700;
        ///	text-transform: uppercase;
        ///	margin: -5px;
        ///	width: 100%;
        ///	height: 100%;
        ///	line-height: 100px;
        ///}
        ///
        ///.popup1 {
        ///	background-color: #00C957;
        ///	padding: 10px;
        ///}
        ///
        ///.popup2 {
        ///	background-color [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string css_stylesheet_css {
            get {
                return ResourceManager.GetString("css\\stylesheet.css", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap icon_close {
            get {
                object obj = ResourceManager.GetObject("icon-close", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///    &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///    &lt;title&gt;Popups&lt;/title&gt;
        ///    &lt;link rel=&quot;shortcut icon&quot; href=&quot;favicon.ico&quot;&gt;
        ///    &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;default&quot;&gt;
        ///    &lt;div class=&quot;main&quot;&gt;
        ///        &lt;h2&gt;POPUPS TEST PAGE&lt;/h2&gt;
        ///        &lt;p&gt;This page is used for demonstrating how popup windows are handled in DotNetBrowser.&lt;/p&gt;
        ///        &lt;p&gt;The links below are examples of the most common popups.&lt;/p&gt;
        ///        &lt;p&gt;&lt;a href=&quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string index_html {
            get {
                return ResourceManager.GetString("index.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to function writeDate() {
        ///
        ///    var now = new Date();
        ///    var days = new Array(&apos;Sunday&apos;, &apos;Monday&apos;, &apos;Tuesday&apos;, &apos;Wednesday&apos;, &apos;Thursday&apos;, &apos;Friday&apos;, &apos;Saturday&apos;);
        ///    var months = new Array(&apos;January&apos;, &apos;February&apos;, &apos;March&apos;, &apos;April&apos;, &apos;May&apos;, &apos;June&apos;, &apos;July&apos;, &apos;August&apos;, &apos;September&apos;, &apos;October&apos;, &apos;November&apos;, &apos;December&apos;);
        ///    var date = ((now.getDate() &lt; 10) ? &quot;0&quot; : &quot;&quot;) + now.getDate();
        ///
        ///    function y2k(number) { return (number &lt; 1000) ? number + 1900 : number; }
        ///
        ///    today = &quot;Popup  &quot; + days[now.getDay()] + &quot; &quot; +
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string js_popups_js {
            get {
                return ResourceManager.GetString("js\\popups.js", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;script src=&quot;js/popups.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///  &lt;script language=&quot;JavaScript&quot;&gt;
        ///    writeDate();
        ///  &lt;/script&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;popup1&quot;&gt;
        ///  &lt;div class=&quot;popup&quot;&gt;Popup 1&lt;/div&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string popup1_html {
            get {
                return ResourceManager.GetString("popup1.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;script src=&quot;js/popups.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///  &lt;script language=&quot;JavaScript&quot;&gt;
        ///    writeDate();
        ///  &lt;/script&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;popup2&quot;&gt;
        ///  &lt;div class=&quot;popup&quot;&gt;Popup 2&lt;/div&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string popup2_html {
            get {
                return ResourceManager.GetString("popup2.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;script src=&quot;js/popups.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///  &lt;script language=&quot;JavaScript&quot;&gt;
        ///    writeDate();
        ///  &lt;/script&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;popup3&quot;&gt;
        ///  &lt;div class=&quot;popup&quot;&gt;Popup 3&lt;/div&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string popup3_html {
            get {
                return ResourceManager.GetString("popup3.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;script src=&quot;js/popups.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///  &lt;script language=&quot;JavaScript&quot;&gt;
        ///    writeDate();
        ///  &lt;/script&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;popup4&quot;&gt;
        ///  &lt;div class=&quot;popup&quot;&gt;Popup 4&lt;/div&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string popup4_html {
            get {
                return ResourceManager.GetString("popup4.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;script src=&quot;js/popups.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///  &lt;script language=&quot;JavaScript&quot;&gt;
        ///    writeDate();
        ///  &lt;/script&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;popup5&quot;&gt;
        ///  &lt;div class=&quot;popup&quot;&gt;Popup 5&lt;/div&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string popup5_html {
            get {
                return ResourceManager.GetString("popup5.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;script src=&quot;js/popups.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///  &lt;script language=&quot;JavaScript&quot;&gt;
        ///    writeDate();
        ///  &lt;/script&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;popup6&quot;&gt;
        ///  &lt;div class=&quot;popup&quot;&gt;Popup 6&lt;/div&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string popup6_html {
            get {
                return ResourceManager.GetString("popup6.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;script src=&quot;js/popups.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///  &lt;script language=&quot;JavaScript&quot;&gt;
        ///    writeDate();
        ///  &lt;/script&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;popup7&quot;&gt;
        ///  &lt;div class=&quot;popup&quot;&gt;Popup 7&lt;/div&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string popup7_html {
            get {
                return ResourceManager.GetString("popup7.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;script src=&quot;js/popups.js&quot; type=&quot;text/javascript&quot;&gt;&lt;/script&gt;
        ///  &lt;script language=&quot;JavaScript&quot;&gt;
        ///    writeDate();
        ///  &lt;/script&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///&lt;/head&gt;
        ///&lt;body class=&quot;popup8&quot;&gt;
        ///  &lt;div class=&quot;popup&quot;&gt;Popup 8&lt;/div&gt;
        ///&lt;/body&gt;
        ///&lt;/html&gt;.
        /// </summary>
        internal static string popup8_html {
            get {
                return ResourceManager.GetString("popup8.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///	&lt;title&gt;Popup Test 1&lt;/title&gt;
        ///	&lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///	&lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///	&lt;script language=&quot;JavaScript&quot;&gt;
        ///		//&lt;!--
        ///		var useHeight = 105;
        ///		function stopError() { return true; }
        ///		window.onerror = stopError;
        ///		function popup(url, yes) {
        ///			if (document.screen) { useHeight = screen.availHeight }
        ///			var bannerX = 5; var bannerY = useHeight - 10;
        ///			window.open(url, yes, &apos;resizeable=no,scroll [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string popuptest1_html {
            get {
                return ResourceManager.GetString("popuptest1.html", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html; charset=UTF-8&quot;&gt;
        ///  &lt;link rel=&quot;stylesheet&quot; type=&apos;text/css&apos; href=&quot;css/stylesheet.css&quot; /&gt;
        ///  &lt;script type=&quot;text/javascript&quot; language=&quot;JavaScript&quot;&gt;
        ///    function enter() {
        ///      window.open(&apos;popup1.html&apos;, &apos;&apos;, &apos;toolbar=no,menubar=no,location=yes,height=320,width=300,left=1&apos;);
        ///    }
        ///
        ///    window.open(&apos;popup2.html&apos;, &apos;&apos;, &apos;toolbar=yes,menubar=yes,location=no,height=450,width=180,left=430&apos;);
        ///
        ///    function leave() {
        ///      window.open(&apos;popup3.h [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string popuptest2_html {
            get {
                return ResourceManager.GetString("popuptest2.html", resourceCulture);
            }
        }
    }
}

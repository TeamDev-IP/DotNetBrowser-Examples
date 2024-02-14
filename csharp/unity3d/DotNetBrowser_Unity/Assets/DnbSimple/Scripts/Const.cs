using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal static class Const
    {
        public static readonly string[] Pages = new[]
        {
            @"www.youtube.com",
#if UNITY_EDITOR
            @".\Assets\DnbSimple\Html\Menu\MenuPage.html",
            @".\Assets\DnbSimple\Html\Chat\Chat.html",
            @".\Assets\DnbSimple\Html\Wiki\Wiki.html",
#else
            @".\DnbSimple\Html\Menu\MenuPage.html",
            @".\DnbSimple\DnbSimple\Html\Chat\Chat.html",
            @".\DnbSimple\DnbSimple\Html\Wiki\Wiki.html",
#endif
        };
    }
}

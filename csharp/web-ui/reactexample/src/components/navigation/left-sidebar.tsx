/*
 *  Copyright (c) 2025 TeamDev
 *
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *
 *  The above copyright notice and this permission notice shall be included in all
 *  copies or substantial portions of the Software.
 *
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *  SOFTWARE.
 */


import {
    Sidebar,
    SidebarContent,
    SidebarGroup,
    SidebarGroupContent,
    SidebarGroupLabel,
    SidebarMenu
} from "@/components/ui/sidebar.tsx";
import {NavigationItem, NavigationItemType} from "@/components/navigation/navigation-item.tsx";
import {Bell, Settings, SquareUser, SunMoon} from "lucide-react";
import {useState} from "react";

/**
 * A sidebar that contains items for navigating between preferences.
 *
 * @constructor
 */
export function LeftSidebar() {
    const [currentNavigation, setCurrentNavigation] = useState<NavigationItemType>("Account");

    const updateNavigation = (navigation: NavigationItemType) => {
        return () => {
            setCurrentNavigation(navigation);
        }
    };
    return (
        <Sidebar className="!h-auto" collapsible={"none"} side="left">
            <SidebarContent className="p-4">
                <SidebarGroup/>
                <SidebarGroupContent>
                    <SidebarMenu>
                        <NavigationItem type={"Account"}
                                        url={"/"}
                                        icon={SquareUser}
                                        isSelected={currentNavigation === "Account"}
                                        onSelect={updateNavigation("Account")}/>
                    </SidebarMenu>
                </SidebarGroupContent>
                <SidebarGroupLabel className={"pt-3"}>Preferences</SidebarGroupLabel>
                <SidebarGroupContent>
                    <SidebarMenu>
                        <NavigationItem type={"General"}
                                        url={"/prefs/general"}
                                        icon={Settings}
                                        isSelected={currentNavigation === "General"}
                                        onSelect={updateNavigation("General")}/>
                        <NavigationItem type={"Appearance"}
                                        url={"/prefs/appearance"}
                                        icon={SunMoon}
                                        isSelected={currentNavigation === "Appearance"}
                                        onSelect={updateNavigation("Appearance")}/>
                        <NavigationItem type={"Notifications"}
                                        url={"/prefs/notifications"}
                                        icon={Bell}
                                        isSelected={currentNavigation === "Notifications"}
                                        onSelect={updateNavigation("Notifications")}/>
                    </SidebarMenu>
                </SidebarGroupContent>
                <SidebarGroup/>
            </SidebarContent>
        </Sidebar>
    )
}

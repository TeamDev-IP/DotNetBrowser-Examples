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

import {SidebarMenuButton, SidebarMenuItem} from "@/components/ui/sidebar.tsx";
import {LucideIcon} from "lucide-react";

/**
 * Available navigation items.
 */
export type NavigationItemType = "Account" | "General" | "Appearance" | "Notifications";

/**
 * The navigation item's properties.
 */
interface NavigationItemProps {
    /**
     * The item's type.
     */
    type: NavigationItemType;
    /**
     * The URL to be navigated to.
     */
    url: string;
    /**
     * The item's icon.
     */
    icon: LucideIcon;
    /**
     * Indicates if the navigation item is selected.
     */
    isSelected: boolean;
    /**
     * A callback invoked when the navigation item has been selected.
     */
    onSelect: () => void;
}

/**
 * A navigation item in the sidebar for switching between preferences.
 *
 * @param props the preferences for the item
 * @constructor
 */
export function NavigationItem(props: NavigationItemProps) {
    return (
        <SidebarMenuItem key={props.type}>
            <SidebarMenuButton isActive={props.isSelected} onClick={props.onSelect} asChild>
                <a href={`#${props.url}`}>
                    <props.icon/>
                    <span>{props.type}</span>
                </a>
            </SidebarMenuButton>
        </SidebarMenuItem>
    );
}

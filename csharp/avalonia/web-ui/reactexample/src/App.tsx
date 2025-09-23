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

import {SidebarProvider} from "@/components/ui/sidebar.tsx";
import {AppearanceProvider} from "@/components/appearance/appearance-provider.tsx";
import {LeftSidebar} from "@/components/navigation/left-sidebar.tsx";
import {PreferencesRouter} from "@/components/navigation/preferences-router.tsx";

/**
 * The main application component.
 *
 * @constructor
 */
function App() {
    return (
        <AppearanceProvider>
            <SidebarProvider className="space-y-2 space-x-2 h-full">
                <LeftSidebar/>
                <main className="w-full p-8">
                    <PreferencesRouter/>
                </main>
            </SidebarProvider>
        </AppearanceProvider>
    );
}

export default App;

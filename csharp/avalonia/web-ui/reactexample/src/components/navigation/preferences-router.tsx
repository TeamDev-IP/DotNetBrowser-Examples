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

import {HashRouter, Route, Routes} from "react-router-dom";
import {UserAccount} from "@/components/account/user-account.tsx";
import {General} from "@/components/general/general.tsx";
import {Appearance} from "@/components/appearance/appearance.tsx";
import {Notifications} from "@/components/notifications/notifications.tsx";

/**
 * A router for navigating between preferences.
 *
 * @constructor
 */
export function PreferencesRouter() {
    return (
        <HashRouter>
            <Routes>
                <Route path={"/"} element={<UserAccount/>}/>
                <Route path={"/prefs/general"} element={<General/>}/>
                <Route path={"/prefs/appearance"} element={<Appearance/>}/>
                <Route path={"/prefs/notifications"}
                       element={<Notifications/>}/>
            </Routes>
        </HashRouter>
    );
}

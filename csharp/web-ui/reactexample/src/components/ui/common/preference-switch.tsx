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

import {Switch} from "@/components/ui/switch.tsx";

/**
 * The switch's properties.
 */
interface PreferenceSwitchProps {
    isChecked: boolean;
    onChange: (value: boolean) => void;
}

/**
 * A switch component for toggling boolean values.
 *
 * @param isChecked indicates whether the switch is in the "checked" state
 * @param onChange a callback function triggered when the switch state changes
 * @constructor
 */
export function PreferenceSwitch({isChecked, onChange}: PreferenceSwitchProps) {
    return <Switch checked={isChecked} onCheckedChange={onChange}
                   className="hover:opacity-70 data-[state=checked]:bg-control-select data-[state=unchecked]:bg-neutral-200"/>;
}

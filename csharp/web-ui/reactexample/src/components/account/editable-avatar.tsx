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

import {Avatar, AvatarFallback, AvatarImage} from "@/components/ui/avatar.tsx";
import {cn} from "@/lib/utils.ts";
import {Upload} from "lucide-react";
import {useRef} from "react";

/**
 * The avatar's properties.
 */
interface EditableAvatarProps {
    pictureDataUri: string,
    fallback: string,
    onChange: (file: File) => void
}

/**
 * A component that represents the user's avatar that can be edited on click.
 *
 * @param pictureDataUri data URI of the avatar image
 * @param fallback a literal value that should be applied when pictureDataUri is not valid
 * @param onChange a callback function that is invoked when the used selects another avatar
 * @constructor
 */
export function EditableAvatar({pictureDataUri, fallback, onChange}: EditableAvatarProps) {
    const fileInputRef = useRef<HTMLInputElement>(null);

    return (
        <div className="w-full items-center flex justify-between">
            <p className="text-sm">Profile picture</p>
            <div onClick={() => {
                fileInputRef.current?.click();
            }}
                 className="flex relative justify-center items-center group">
                <Avatar
                    className="group-hover:opacity-60 transition-colors duration-200 object-cover">
                    <AvatarImage src={pictureDataUri}/>
                    <AvatarFallback>{fallback}</AvatarFallback>
                </Avatar>
                <input
                    type="file"
                    ref={fileInputRef}
                    accept="image/*"
                    className="hidden"
                    onChange={event => {
                        const file = event.target.files?.[0];
                        if (file) {
                            onChange(file);
                        }
                    }}
                />
                <div className={cn(
                    "absolute inset-0 flex items-center justify-center text-primary text-sm font-medium opacity-0 transition-opacity group-hover:opacity-100"
                )}>
                    <Upload/>
                </div>
            </div>
        </div>
    );
}

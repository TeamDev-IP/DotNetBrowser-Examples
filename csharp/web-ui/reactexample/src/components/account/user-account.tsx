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

import {Combobox} from "@/components/ui/common/combobox.tsx";
import {PreferenceSwitch} from "@/components/ui/common/preference-switch.tsx";
import {EditableAvatar} from "@/components/account/editable-avatar.tsx";
import {EditableInput} from "@/components/account/editable-input.tsx";
import {GuidingLine} from "@/components/ui/common/guiding-line.tsx";
import {useEffect, useState} from "react";
import {prefsClient} from "@/rpc/prefs-client.ts";
import {AccountSchema, ProfilePictureSchema} from "@/gen/prefs_pb.ts";
import {create} from "@bufbuild/protobuf";
import {imageToDataUri} from "@/converter/image.ts";
import {
    emailTwoFA,
    fromTfa,
    passkeyTwoFA,
    smsTwoFA,
    toTfa,
    TwoFAMethod
} from "@/converter/two-fa-method.ts";
import {prefsStorage} from "@/storage/prefs-storage.ts";

/**
 * Available two-factor authentication methods.
 */
const authentications: TwoFAMethod[] = [
    emailTwoFA,
    smsTwoFA,
    passkeyTwoFA
];

/**
 * A component that allows managing the user's account.
 *
 * @constructor
 */
export function UserAccount() {
    const [profilePictureDataUri, setProfilePictureDataUri] = useState<string>("");
    const [fullName, setFullName] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [twoFactorAuthentication, setTwoFactorAuthentication] =
        useState<TwoFAMethod>(prefsStorage.twoFA());
    const [biometricAuthentication, setBiometricAuthentication] =
        useState<boolean>(prefsStorage.biometricAuthenticationEnabled());

    useEffect(() => {
        (async () => {
            const account = await prefsClient.getAccount({});
            setEmail(account.email);
            setFullName(account.fullName);

            const tfaMethod = fromTfa(account.twoFactorAuthentication);
            setTwoFactorAuthentication(tfaMethod);
            setBiometricAuthentication(account.biometricAuthentication);

            prefsStorage.saveTwoFA(tfaMethod);
            prefsStorage.saveBiometricAuthentication(account.biometricAuthentication);
            const profilePicture = await prefsClient.getProfilePicture({});
            setProfilePictureDataUri(imageToDataUri(profilePicture.content));
        })();
    }, []);

    const onUpdateAccount = ({
                                 newFullName = fullName,
                                 newEmail = email,
                                 newTwoFactorAuthentication = twoFactorAuthentication,
                                 newBiometricAuthentication = biometricAuthentication
                             }: {
        newFullName?: string;
        newEmail?: string,
        newTwoFactorAuthentication?: TwoFAMethod,
        newBiometricAuthentication?: boolean
    }) => {
        const newAccount = create(AccountSchema, {
            fullName: newFullName,
            email: newEmail,
            twoFactorAuthentication: toTfa(newTwoFactorAuthentication),
            biometricAuthentication: newBiometricAuthentication
        });
        prefsClient.setAccount(newAccount);
        prefsStorage.saveTwoFA(newTwoFactorAuthentication);
        prefsStorage.saveBiometricAuthentication(newBiometricAuthentication);
    };
    const onChangeAvatar = (file: File) => {
        const reader = new FileReader();
        reader.onload = () => {
            const newProfilePicture = create(ProfilePictureSchema, {
                content: new Uint8Array(reader.result as ArrayBuffer)
            });
            prefsClient.setProfilePicture(newProfilePicture);
            setProfilePictureDataUri(imageToDataUri(newProfilePicture.content));
        };
        reader.readAsArrayBuffer(file);
    };
    return (
        <div className="space-y-4">
            <h1 className="text-2xl font-semibold">Account</h1>
            <GuidingLine/>
            <EditableAvatar onChange={onChangeAvatar} pictureDataUri={profilePictureDataUri}
                            fallback={fullName.split(" ").map(it => it[0]).join("")}/>
            <EditableInput title={"Email"} isEmail={true}
                           onChange={(value) => {
                               onUpdateAccount({newEmail: value});
                               setEmail(value);
                           }} value={email} id={"email"}/>
            <EditableInput title={"Full name"} isEmail={false} value={fullName}
                           onChange={(value) => {
                               onUpdateAccount({newFullName: value});
                               setFullName(value);
                           }} id={"fullname"}/>
            <GuidingLine/>
            <div className="w-full inline-flex items-center space-y-2 justify-between py-1">
                <div className="pr-8">
                    <p className="text-sm">Two-factor authentication</p>
                    <p className="text-xs text-muted-foreground text-gray-500">
                        Select an extra layer of security by requiring a code when logging in.
                    </p>
                </div>
                <Combobox onSelect={value => {
                    onUpdateAccount({newTwoFactorAuthentication: value as TwoFAMethod});
                    setTwoFactorAuthentication(value as TwoFAMethod);
                }} options={authentications}
                          currentOption={twoFactorAuthentication}/>
            </div>
            <div className="w-full inline-flex items-center justify-between py-1">
                <div className="pr-8">
                    <p className="text-sm">Biometric authentication</p>
                    <p className="text-xs text-muted-foreground text-gray-500">
                        Allow authentication via fingerprints or Face ID.
                    </p>
                </div>
                <PreferenceSwitch onChange={checked => {
                    onUpdateAccount({newBiometricAuthentication: checked});
                    setBiometricAuthentication(checked);
                }} isChecked={biometricAuthentication}/>
            </div>
        </div>
    );
}

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

import {TwoFactorAuthentication} from "@/gen/prefs_pb.ts";

/**
 * The two-factor authentication methods as human-readable strings.
 */
export type TwoFAMethod = "Email" | "SMS" | "Passkey"

const emailTwoFA: TwoFAMethod = "Email";
const smsTwoFA: TwoFAMethod = "SMS";
const passkeyTwoFA: TwoFAMethod = "Passkey";

/**
 * Converts {@link TwoFactorAuthentication} to TfaMethod.
 */
function fromTfa(value: TwoFactorAuthentication): TwoFAMethod {
    if (value === TwoFactorAuthentication.EMAIL) {
        return emailTwoFA;
    } else if (value === TwoFactorAuthentication.SMS) {
        return smsTwoFA;
    } else if (value === TwoFactorAuthentication.PASS_KEY) {
        return passkeyTwoFA;
    } else {
        throw new TypeError("Incorrect two-factor authentication.");
    }
}

/**
 * Converts TwoFactorAuthentication to {@link TwoFAMethod}.
 */
function toTfa(value: TwoFAMethod): TwoFactorAuthentication {
    if (value === emailTwoFA) {
        return TwoFactorAuthentication.EMAIL;
    } else if (value === smsTwoFA) {
        return TwoFactorAuthentication.SMS;
    } else {
        return TwoFactorAuthentication.PASS_KEY;
    }
}

export {
    emailTwoFA,
    smsTwoFA,
    passkeyTwoFA,
    fromTfa,
    toTfa
};

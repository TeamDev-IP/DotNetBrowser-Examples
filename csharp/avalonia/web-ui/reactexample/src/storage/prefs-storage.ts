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

import {emailTwoFA, TwoFAMethod} from "@/converter/two-fa-method.ts";
import {englishLanguage, LanguageOption} from "@/converter/language.ts";
import {defaultFontSize, FontSizeOption} from "@/converter/font-size.ts";
import {systemTheme, ThemeOption} from "@/converter/theme.ts";

/**
 * A key for the two-factor authentication in the local storage.
 */
const twoFAKey = "two-factor-authentication";
/**
 * A key for the biometric authentication in the local storage.
 */
const biometricAuthenticationKey = "biometric-authentication";
/**
 * A key for the launch-at-startup preference in the local storage.
 */
const launchAtStartupKey = "launch-at-startup";
/**
 * A key for the check-for-updates preference in the local storage.
 */
const checkForUpdatesKey = "check-for-updates";
/**
 * A key for the language in the local storage.
 */
const languageKey = "language";
/**
 * A key for the font size in the local storage.
 */
const fontSizeKey = "font-size";
/**
 * A key for the theme in the local storage.
 */
const themeKey = "vite-ui-theme";
/**
 * A key for the email notifications in the local storage.
 */
const emailNotificationsKey = "email-notifications";
/**
 * A key for the desktop notifications in the local storage.
 */
const desktopNotificationsKey = "desktop-notifications";

/**
 * A wrapper over the local storage to store the preferences.
 */
class PrefsStorage {

    /**
     * Reads the two-factor authentication method from the local storage.
     */
    twoFA() {
        return localStorage.getItem(twoFAKey) as TwoFAMethod || emailTwoFA;
    }

    /**
     * Saves a new two-factor authentication method to the local storage.
     */
    saveTwoFA(tfaMethod: TwoFAMethod) {
        localStorage.setItem(twoFAKey, tfaMethod);
    }

    /**
     * Returns true if the biometric authentication is enabled.
     */
    biometricAuthenticationEnabled() {
        return localStorage.getItem(biometricAuthenticationKey) === "true" || false;
    }

    /**
     * Saves "true" to the local storage if the biometric authentication is enabled.
     */
    saveBiometricAuthentication(isEnabled: boolean) {
        localStorage.setItem(biometricAuthenticationKey, isEnabled ? "true" : "false");
    }

    /**
     * Returns true if the launch-at-startup preference is enabled.
     */
    launchAtStartupEnabled() {
        return localStorage.getItem(launchAtStartupKey) === "true" || false;
    }

    /**
     * Saves "true" to the local storage if the launch-at-startup preference is enabled.
     */
    saveLaunchAtStartup(isEnabled: boolean) {
        localStorage.setItem(launchAtStartupKey, isEnabled ? "true" : "false");
    }

    /**
     * Returns true if the check-for-updates preference is enabled.
     */
    checkForUpdatesEnabled() {
        return localStorage.getItem(checkForUpdatesKey) === "true" || false;
    }

    /**
     * Saves "true" to the local storage if the check-for-updates preference is enabled.
     */
    saveCheckForUpdates(isEnabled: boolean) {
        localStorage.setItem(checkForUpdatesKey, isEnabled ? "true" : "false");
    }

    /**
     * Reads the language from the local storage.
     */
    language() {
        return localStorage.getItem(languageKey) as LanguageOption || englishLanguage;
    }

    /**
     * Saves a new language to the local storage.
     */
    saveLanguage(language: LanguageOption) {
        localStorage.setItem(languageKey, language);
    }

    /**
     * Reads the theme from the local storage.
     */
    theme() {
        return localStorage.getItem(themeKey) as ThemeOption || systemTheme;
    }

    /**
     * Saves a new theme to the local storage.
     */
    saveTheme(language: ThemeOption) {
        localStorage.setItem(themeKey, language);
    }

    /**
     * Reads the font size from the local storage.
     */
    fontSize() {
        return localStorage.getItem(fontSizeKey) as FontSizeOption || defaultFontSize;
    }

    /**
     * Saves a new font size to the local storage.
     */
    saveFontSize(option: FontSizeOption) {
        localStorage.setItem(fontSizeKey, option);
    }

    /**
     * Returns true if the email notifications are enabled.
     */
    emailNotificationsEnabled() {
        return localStorage.getItem(emailNotificationsKey) === "true" || false;
    }

    /**
     * Saves "true" to the local storage if the email notifications are enabled.
     */
    saveEmailNotifications(isEnabled: boolean) {
        localStorage.setItem(emailNotificationsKey, isEnabled ? "true" : "false");
    }

    /**
     * Returns true if the desktop notifications are enabled.
     */
    desktopNotificationsEnabled() {
        return localStorage.getItem(desktopNotificationsKey) === "true" || false;
    }

    /**
     * Saves "true" to the local storage if the desktop notifications are enabled.
     */
    saveDesktopNotifications(isEnabled: boolean) {
        localStorage.setItem(desktopNotificationsKey, isEnabled ? "true" : "false");
    }
}

const prefsStorage = new PrefsStorage();

export {
    prefsStorage
}

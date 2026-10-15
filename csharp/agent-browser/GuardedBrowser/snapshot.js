/*
 *  Copyright (c) 2026 TeamDev
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

// Lists the interactive elements of the page with numbered refs, so the
// assistant can act on "[3]" instead of pixel coordinates. DotNetBrowser has
// no accessibility-tree API, so the list is built from the DOM.
(() => {
  const selector = 'a[href], button, input, select, textarea, '
    + '[role="button"], [role="link"]';

  const visible = el => {
    const r = el.getBoundingClientRect();
    const s = getComputedStyle(el);
    return r.width > 0 && r.height > 0
      && s.visibility !== 'hidden' && s.display !== 'none';
  };

  const labelOf = el => {
    if (el.labels && el.labels.length) return el.labels[0].innerText.trim();
    return el.getAttribute('aria-label') || el.placeholder || el.name || '';
  };

  const textOf = el => (el.innerText || el.value || el.alt || '')
    .trim().replace(/\s+/g, ' ').slice(0, 80);

  const roleOf = (el, tag, type) => {
    if (tag === 'a') return 'link';
    if (tag === 'button') return 'button';
    if (tag === 'select') return 'combobox';
    if (tag === 'textarea') return 'textbox';
    if (tag === 'input') {
      if (type === 'checkbox' || type === 'radio') return type;
      if (type === 'submit' || type === 'image' || type === 'button') return 'button';
      return 'textbox';
    }
    return el.getAttribute('role') || tag;
  };

  const isSubmitButton = (el, tag, type) => {
    if (!el.form) return false;
    if (tag === 'button') return type !== 'button' && type !== 'reset';
    return tag === 'input' && (type === 'submit' || type === 'image');
  };

  document.querySelectorAll('[data-agent-ref]')
    .forEach(el => el.removeAttribute('data-agent-ref'));
  const elements = [];
  let ref = 0;
  for (const el of document.querySelectorAll(selector)) {
    if (!visible(el)) continue;
    ref++;
    el.setAttribute('data-agent-ref', ref);
    const tag = el.tagName.toLowerCase();
    const type = (el.getAttribute('type') || '').toLowerCase();
    const role = roleOf(el, tag, type);
    const isField = role === 'textbox' || role === 'combobox';
    elements.push({
      ref,
      role,
      text: isField ? labelOf(el) : (textOf(el) || labelOf(el)),
      value: role === 'textbox' ? (type === 'password' ? '' : el.value) : undefined,
      href: tag === 'a' ? el.href : undefined,
      isPassword: type === 'password',
      isSubmit: isSubmitButton(el, tag, type),
      disabled: !!el.disabled
    });
  }
  const text = (document.body ? document.body.innerText : '')
    .replace(/\n{3,}/g, '\n\n').slice(0, 3000);
  return JSON.stringify({ title: document.title, url: location.href, elements, text });
})()

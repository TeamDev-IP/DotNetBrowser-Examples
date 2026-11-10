// The .NET side calls the functions of window.editor and learns about changes
// through window.host, the object it injects into the page.
const quill = new Quill('#editor', {
    theme: 'snow',
    modules: { toolbar: false },
    // Only these formats survive opening or pasting a document. Video and
    // formula are left out on purpose: with video, a crafted file could put
    // script into the saved HTML, and a formula makes Quill throw an error,
    // because the page doesn't load KaTeX.
    formats: ['bold', 'italic', 'underline', 'header', 'list', 'link',
              'color', 'image'],
    placeholder: 'Start writing...'
});

window.editor = {
    // Quill 2.0.3 writes every space as &nbsp;, so turn them back.
    getHtml() {
        return quill.getSemanticHTML().replaceAll('&nbsp;', ' ');
    },
    setHtml(html) {
        quill.setContents(quill.clipboard.convert({ html }), 'silent');
        quill.history.clear();
    },
    // Turns a format on for the selection, or off if it is already on.
    toggle(name, value = true) {
        const current = quill.getFormat()[name];
        quill.format(name, current === value ? false : value, 'user');
    }
};

quill.on('text-change', () => {
    const words = quill.getText().split(/\s+/).filter(w => w.length > 0);
    window.host.TextChanged(words.length);
});

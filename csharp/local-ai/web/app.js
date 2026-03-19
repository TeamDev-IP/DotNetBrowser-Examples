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

import { pipeline } from "https://cdn.jsdelivr.net/npm/@huggingface/transformers@3.8.1";

const MODEL_ID = "Xenova/LaMini-Flan-T5-783M";
const MODEL_LABEL = "LaMini-Flan-T5-783M";
const buttons = [...document.querySelectorAll("button[data-action]")];
const input = document.querySelector("#input");
const output = document.querySelector("#output");
const status = document.querySelector("#status");
const backend = document.querySelector("#backend");

const prompts = {
  summarize: (text) => `Summarize this text in 3 bullet points: ${text}`,
  shorten: (text) => `Shorten this text while preserving the meaning: ${text}`,
  rephrase: (text) => `Rephrase this text in clearer language: ${text}`,
};

let generator;

setBusy(true);
initialize().catch((error) => {
  console.error(error);
  status.textContent = "Load failed.";
  output.textContent = error.message;
  setBusy(false);
});

async function initialize() {
  generator = await createGenerator();
  status.textContent = MODEL_LABEL;
  input.value = `Paste text here and try one of the actions.`;
  output.textContent = "Your result will appear here.";

  for (const button of buttons) {
    button.addEventListener("click", () => runAction(button.dataset.action));
  }

  setBusy(false);
}

async function createGenerator() {
  if (navigator.gpu) {
    try {
      backend.textContent = "WebGPU";
      status.textContent = "Loading…";
      return await pipeline("text2text-generation", MODEL_ID, {
        device: "webgpu",
        dtype: "q4",
      });
    } catch (error) {
      console.warn("WebGPU initialization failed, falling back.", error);
    }
  }

  backend.textContent = "CPU";
  status.textContent = "Loading…";
  return await pipeline("text2text-generation", MODEL_ID, {
    dtype: "q4",
  });
}

async function runAction(action) {
  const sourceText = input.value.trim();
  if (!sourceText) {
    status.textContent = "Add text first.";
    output.textContent = "";
    return;
  }

  setBusy(true);
  status.textContent = "Running…";

  try {
    const result = await generator(prompts[action](sourceText), {
      max_new_tokens: 128,
      temperature: 0.2,
      do_sample: false,
    });

    output.textContent = result[0].generated_text.trim();
    status.textContent = MODEL_LABEL;
  } catch (error) {
    console.error(error);
    status.textContent = "Run failed.";
    output.textContent = error.message;
  } finally {
    setBusy(false);
  }
}

function setBusy(isBusy) {
  for (const button of buttons) {
    button.disabled = isBusy;
  }
}

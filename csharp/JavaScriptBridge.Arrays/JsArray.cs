#region Copyright

// Copyright © 2022, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using DotNetBrowser.Js;

namespace JavaScriptBridge.Arrays
{
    public static class JsObjectExtensions
    {
        public static JsArray AsArray(this IJsObject jsObject) => JsArray.AsArray(jsObject);
    }

    public class JsArray : IReadOnlyList<object>
    {
        private readonly IJsObject jsObject;

        public int Count => Convert.ToInt32((double) jsObject.Properties["length"]);

        public object this[int index] => jsObject.Properties[(uint) index];

        private JsArray(IJsObject jsObject)
        {
            this.jsObject = jsObject;
        }

        public IEnumerator<object> GetEnumerator() => new JsArrayEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static JsArray AsArray(IJsObject jsObject) => !IsArray(jsObject) ? null : new JsArray(jsObject);

        private static bool IsArray(IJsObject jsObject)
        {
            if (jsObject == null || jsObject.IsDisposed)
            {
                return false;
            }

            IJsFunction isArrayFunction = jsObject.Frame.ExecuteJavaScript<IJsFunction>("Array.isArray").Result;
            return isArrayFunction.Invoke<bool>(null, jsObject);
        }

        private class JsArrayEnumerator : IEnumerator<object>
        {
            private readonly int count;
            private readonly JsArray jsArray;
            private int index;

            public object Current => jsArray[index];

            object IEnumerator.Current => Current;

            public JsArrayEnumerator(JsArray jsArray)
            {
                this.jsArray = jsArray;
                count = jsArray.Count;
                index = -1;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (index >= count - 1)
                {
                    return false;
                }

                index++;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }
        }
    }
}

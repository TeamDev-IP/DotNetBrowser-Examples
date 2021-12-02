#region Copyright

// Copyright © 2021, TeamDev. All rights reserved.
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
using System.Threading.Tasks;
using DotNetBrowser.Js;

namespace JavaScriptBridge.Promises
{
    public static class JsObjectExtensions
    {
        public static JsPromise AsPromise(this IJsObject jsObject) => JsPromise.AsPromise(jsObject);
    }

    /// <summary>
    ///     An example of a wrapper for IJsObject that simplifies handling JavaScript promises.
    /// </summary>
    public sealed class JsPromise
    {
        private readonly IJsObject jsObject;

        private JsPromise(IJsObject jsObject)
        {
            this.jsObject = jsObject;
        }

        /// <summary>
        ///     Creates a JavaScript promise representation of the IJsObject instance.
        /// </summary>
        /// <param name="jsObject">The JavaScript object.</param>
        /// <returns>The JsPromise representation of the object.</returns>
        public static JsPromise AsPromise(IJsObject jsObject) => !IsPromise(jsObject) ? null : new JsPromise(jsObject);

        /// <summary>
        ///     Appends a rejection handler callback to the promise, and returns a new <see cref="JsPromise" />
        ///     resolving to the return value of the callback if it is called, or to its original fulfillment
        ///     value if the promise is instead fulfilled.
        /// </summary>
        /// <param name="onRejected">The rejection handler.</param>
        /// <returns>A new <see cref="JsPromise" /></returns>
        public JsPromise Catch(Func<object, object> onRejected)
        {
            IJsObject newPromise = jsObject.Invoke("catch", onRejected) as IJsObject;
            return new JsPromise(newPromise);
        }

        /// <summary>
        ///     Appends fulfillment and rejection handlers to the promise, and returns a <c>Task</c>
        ///     that becomes completed as soon as the promise is resolved.
        /// </summary>
        /// <returns>
        ///     a <c>Task</c> that becomes completed as soon as any of these handlers is called.
        /// </returns>
        public Task<Result> ResolveAsync()
        {
            TaskCompletionSource<Result> promiseTcs = new TaskCompletionSource<Result>();
            Then(obj => { promiseTcs.TrySetResult(Fulfilled(obj)); },
                 obj => { promiseTcs.TrySetResult(Rejected(obj)); });

            promiseTcs.Task.ConfigureAwait(false);
            return promiseTcs.Task;
        }

        /// <summary>
        ///     Appends fulfillment and rejection handlers to the promise.
        /// </summary>
        /// <param name="onFulfilled"> The fulfillment handler.</param>
        /// <param name="onRejected">The rejection handler.</param>
        public void Then(Action<object> onFulfilled, Action<object> onRejected = null)
        {
            jsObject.Invoke("then", onFulfilled, onRejected);
        }

        /// <summary>
        ///     Appends fulfillment and rejection handlers to the promise, and returns a new <see cref="JsPromise" />
        ///     resolving to the return value of the called handler, or to its original settled value
        ///     if the promise was not handled.
        /// </summary>
        /// <param name="onFulfilled"> The fulfillment handler.</param>
        /// <param name="onRejected">The rejection handler.</param>
        /// <returns>A new <see cref="JsPromise" /></returns>
        public JsPromise Then(Func<object, object> onFulfilled, Func<object, object> onRejected = null)
        {
            IJsObject newPromise = jsObject.Invoke("then", onFulfilled, onRejected) as IJsObject;
            return new JsPromise(newPromise);
        }

        private Result Fulfilled(object o) => new Result(ResultState.Fulfilled, o);

        private static bool IsPromise(IJsObject jsObject)
        {
            if (jsObject == null || jsObject.IsDisposed)
            {
                return false;
            }

            IJsObject promisePrototype = jsObject.Frame.ExecuteJavaScript<IJsObject>("Promise.prototype").Result;
            return promisePrototype.Invoke<bool>("isPrototypeOf", jsObject);
        }

        private Result Rejected(object o) => new Result(ResultState.Rejected, o);

        /// <summary>
        ///     The Promise result state.
        /// </summary>
        public enum ResultState
        {
            /// <summary>
            ///     The Promise was fulfilled.
            /// </summary>
            Fulfilled,

            /// <summary>
            ///     The promise was rejected.
            /// </summary>
            Rejected
        }

        public class Result
        {
            /// <summary>
            ///     The object passed to the fulfillment or rejection handler.
            /// </summary>
            public object Data { get; }

            /// <summary>
            ///     The promise result state that indicates whether the promise was fulfilled or rejected.
            /// </summary>
            public ResultState State { get; }

            internal Result(ResultState state, object data)
            {
                State = state;
                Data = data;
            }
        }
    }
}
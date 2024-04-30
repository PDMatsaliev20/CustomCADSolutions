"use strict";
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
exports.DefaultHttpClient = void 0;
var Errors_1 = require("./Errors");
var FetchHttpClient_1 = require("./FetchHttpClient");
var HttpClient_1 = require("./HttpClient");
var Utils_1 = require("./Utils");
var XhrHttpClient_1 = require("./XhrHttpClient");
/** Default implementation of {@link @microsoft/signalr.HttpClient}. */
var DefaultHttpClient = /** @class */ (function (_super) {
    __extends(DefaultHttpClient, _super);
    /** Creates a new instance of the {@link @microsoft/signalr.DefaultHttpClient}, using the provided {@link @microsoft/signalr.ILogger} to log messages. */
    function DefaultHttpClient(logger) {
        var _this = _super.call(this) || this;
        if (typeof fetch !== "undefined" || Utils_1.Platform.isNode) {
            _this._httpClient = new FetchHttpClient_1.FetchHttpClient(logger);
        }
        else if (typeof XMLHttpRequest !== "undefined") {
            _this._httpClient = new XhrHttpClient_1.XhrHttpClient(logger);
        }
        else {
            throw new Error("No usable HttpClient found.");
        }
        return _this;
    }
    /** @inheritDoc */
    DefaultHttpClient.prototype.send = function (request) {
        // Check that abort was not signaled before calling send
        if (request.abortSignal && request.abortSignal.aborted) {
            return Promise.reject(new Errors_1.AbortError());
        }
        if (!request.method) {
            return Promise.reject(new Error("No method defined."));
        }
        if (!request.url) {
            return Promise.reject(new Error("No url defined."));
        }
        return this._httpClient.send(request);
    };
    DefaultHttpClient.prototype.getCookieString = function (url) {
        return this._httpClient.getCookieString(url);
    };
    return DefaultHttpClient;
}(HttpClient_1.HttpClient));
exports.DefaultHttpClient = DefaultHttpClient;
//# sourceMappingURL=DefaultHttpClient.js.map
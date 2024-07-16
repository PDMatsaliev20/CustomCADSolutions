// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
import { AbortController } from "./AbortController";
import { HttpError, TimeoutError } from "./Errors";
import { LogLevel } from "./ILogger";
import { TransferFormat } from "./ITransport";
import { Arg, getDataDetail, getUserAgentHeader, sendMessage } from "./Utils";
// Not exported from 'index', this type is internal.
/** @private */
var LongPollingTransport = /** @class */ (function () {
    function LongPollingTransport(httpClient, logger, options) {
        this._httpClient = httpClient;
        this._logger = logger;
        this._pollAbort = new AbortController();
        this._options = options;
        this._running = false;
        this.onreceive = null;
        this.onclose = null;
    }
    Object.defineProperty(LongPollingTransport.prototype, "pollAborted", {
        // This is an internal type, not exported from 'index' so this is really just internal.
        get: function () {
            return this._pollAbort.aborted;
        },
        enumerable: false,
        configurable: true
    });
    LongPollingTransport.prototype.connect = function (url, transferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var _a, name, value, headers, pollOptions, pollUrl, response;
            var _b;
            return __generator(this, function (_c) {
                switch (_c.label) {
                    case 0:
                        Arg.isRequired(url, "url");
                        Arg.isRequired(transferFormat, "transferFormat");
                        Arg.isIn(transferFormat, TransferFormat, "transferFormat");
                        this._url = url;
                        this._logger.log(LogLevel.Trace, "(LongPolling transport) Connecting.");
                        // Allow binary format on Node and Browsers that support binary content (indicated by the presence of responseType property)
                        if (transferFormat === TransferFormat.Binary &&
                            (typeof XMLHttpRequest !== "undefined" && typeof new XMLHttpRequest().responseType !== "string")) {
                            throw new Error("Binary protocols over XmlHttpRequest not implementing advanced features are not supported.");
                        }
                        _a = getUserAgentHeader(), name = _a[0], value = _a[1];
                        headers = __assign((_b = {}, _b[name] = value, _b), this._options.headers);
                        pollOptions = {
                            abortSignal: this._pollAbort.signal,
                            headers: headers,
                            timeout: 100000,
                            withCredentials: this._options.withCredentials,
                        };
                        if (transferFormat === TransferFormat.Binary) {
                            pollOptions.responseType = "arraybuffer";
                        }
                        pollUrl = "".concat(url, "&_=").concat(Date.now());
                        this._logger.log(LogLevel.Trace, "(LongPolling transport) polling: ".concat(pollUrl, "."));
                        return [4 /*yield*/, this._httpClient.get(pollUrl, pollOptions)];
                    case 1:
                        response = _c.sent();
                        if (response.statusCode !== 200) {
                            this._logger.log(LogLevel.Error, "(LongPolling transport) Unexpected response code: ".concat(response.statusCode, "."));
                            // Mark running as false so that the poll immediately ends and runs the close logic
                            this._closeError = new HttpError(response.statusText || "", response.statusCode);
                            this._running = false;
                        }
                        else {
                            this._running = true;
                        }
                        this._receiving = this._poll(this._url, pollOptions);
                        return [2 /*return*/];
                }
            });
        });
    };
    LongPollingTransport.prototype._poll = function (url, pollOptions) {
        return __awaiter(this, void 0, void 0, function () {
            var pollUrl, response, e_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        _a.trys.push([0, , 7, 8]);
                        _a.label = 1;
                    case 1:
                        if (!this._running) return [3 /*break*/, 6];
                        _a.label = 2;
                    case 2:
                        _a.trys.push([2, 4, , 5]);
                        pollUrl = "".concat(url, "&_=").concat(Date.now());
                        this._logger.log(LogLevel.Trace, "(LongPolling transport) polling: ".concat(pollUrl, "."));
                        return [4 /*yield*/, this._httpClient.get(pollUrl, pollOptions)];
                    case 3:
                        response = _a.sent();
                        if (response.statusCode === 204) {
                            this._logger.log(LogLevel.Information, "(LongPolling transport) Poll terminated by server.");
                            this._running = false;
                        }
                        else if (response.statusCode !== 200) {
                            this._logger.log(LogLevel.Error, "(LongPolling transport) Unexpected response code: ".concat(response.statusCode, "."));
                            // Unexpected status code
                            this._closeError = new HttpError(response.statusText || "", response.statusCode);
                            this._running = false;
                        }
                        else {
                            // Process the response
                            if (response.content) {
                                this._logger.log(LogLevel.Trace, "(LongPolling transport) data received. ".concat(getDataDetail(response.content, this._options.logMessageContent), "."));
                                if (this.onreceive) {
                                    this.onreceive(response.content);
                                }
                            }
                            else {
                                // This is another way timeout manifest.
                                this._logger.log(LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                            }
                        }
                        return [3 /*break*/, 5];
                    case 4:
                        e_1 = _a.sent();
                        if (!this._running) {
                            // Log but disregard errors that occur after stopping
                            this._logger.log(LogLevel.Trace, "(LongPolling transport) Poll errored after shutdown: ".concat(e_1.message));
                        }
                        else {
                            if (e_1 instanceof TimeoutError) {
                                // Ignore timeouts and reissue the poll.
                                this._logger.log(LogLevel.Trace, "(LongPolling transport) Poll timed out, reissuing.");
                            }
                            else {
                                // Close the connection with the error as the result.
                                this._closeError = e_1;
                                this._running = false;
                            }
                        }
                        return [3 /*break*/, 5];
                    case 5: return [3 /*break*/, 1];
                    case 6: return [3 /*break*/, 8];
                    case 7:
                        this._logger.log(LogLevel.Trace, "(LongPolling transport) Polling complete.");
                        // We will reach here with pollAborted==false when the server returned a response causing the transport to stop.
                        // If pollAborted==true then client initiated the stop and the stop method will raise the close event after DELETE is sent.
                        if (!this.pollAborted) {
                            this._raiseOnClose();
                        }
                        return [7 /*endfinally*/];
                    case 8: return [2 /*return*/];
                }
            });
        });
    };
    LongPollingTransport.prototype.send = function (data) {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                if (!this._running) {
                    return [2 /*return*/, Promise.reject(new Error("Cannot send until the transport is connected"))];
                }
                return [2 /*return*/, sendMessage(this._logger, "LongPolling", this._httpClient, this._url, data, this._options)];
            });
        });
    };
    LongPollingTransport.prototype.stop = function () {
        return __awaiter(this, void 0, void 0, function () {
            var headers, _a, name_1, value, deleteOptions, error, err_1;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        this._logger.log(LogLevel.Trace, "(LongPolling transport) Stopping polling.");
                        // Tell receiving loop to stop, abort any current request, and then wait for it to finish
                        this._running = false;
                        this._pollAbort.abort();
                        _b.label = 1;
                    case 1:
                        _b.trys.push([1, , 7, 8]);
                        return [4 /*yield*/, this._receiving];
                    case 2:
                        _b.sent();
                        // Send DELETE to clean up long polling on the server
                        this._logger.log(LogLevel.Trace, "(LongPolling transport) sending DELETE request to ".concat(this._url, "."));
                        headers = {};
                        _a = getUserAgentHeader(), name_1 = _a[0], value = _a[1];
                        headers[name_1] = value;
                        deleteOptions = {
                            headers: __assign(__assign({}, headers), this._options.headers),
                            timeout: this._options.timeout,
                            withCredentials: this._options.withCredentials,
                        };
                        error = void 0;
                        _b.label = 3;
                    case 3:
                        _b.trys.push([3, 5, , 6]);
                        return [4 /*yield*/, this._httpClient.delete(this._url, deleteOptions)];
                    case 4:
                        _b.sent();
                        return [3 /*break*/, 6];
                    case 5:
                        err_1 = _b.sent();
                        error = err_1;
                        return [3 /*break*/, 6];
                    case 6:
                        if (error) {
                            if (error instanceof HttpError) {
                                if (error.statusCode === 404) {
                                    this._logger.log(LogLevel.Trace, "(LongPolling transport) A 404 response was returned from sending a DELETE request.");
                                }
                                else {
                                    this._logger.log(LogLevel.Trace, "(LongPolling transport) Error sending a DELETE request: ".concat(error));
                                }
                            }
                        }
                        else {
                            this._logger.log(LogLevel.Trace, "(LongPolling transport) DELETE request accepted.");
                        }
                        return [3 /*break*/, 8];
                    case 7:
                        this._logger.log(LogLevel.Trace, "(LongPolling transport) Stop finished.");
                        // Raise close event here instead of in polling
                        // It needs to happen after the DELETE request is sent
                        this._raiseOnClose();
                        return [7 /*endfinally*/];
                    case 8: return [2 /*return*/];
                }
            });
        });
    };
    LongPollingTransport.prototype._raiseOnClose = function () {
        if (this.onclose) {
            var logMessage = "(LongPolling transport) Firing onclose event.";
            if (this._closeError) {
                logMessage += " Error: " + this._closeError;
            }
            this._logger.log(LogLevel.Trace, logMessage);
            this.onclose(this._closeError);
        }
    };
    return LongPollingTransport;
}());
export { LongPollingTransport };

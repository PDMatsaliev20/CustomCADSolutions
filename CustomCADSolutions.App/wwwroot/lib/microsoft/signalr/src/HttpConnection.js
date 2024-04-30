"use strict";
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
Object.defineProperty(exports, "__esModule", { value: true });
exports.TransportSendQueue = exports.HttpConnection = void 0;
var DefaultHttpClient_1 = require("./DefaultHttpClient");
var Errors_1 = require("./Errors");
var HeaderNames_1 = require("./HeaderNames");
var ILogger_1 = require("./ILogger");
var ITransport_1 = require("./ITransport");
var LongPollingTransport_1 = require("./LongPollingTransport");
var ServerSentEventsTransport_1 = require("./ServerSentEventsTransport");
var Utils_1 = require("./Utils");
var WebSocketTransport_1 = require("./WebSocketTransport");
var MAX_REDIRECTS = 100;
/** @private */
var HttpConnection = /** @class */ (function () {
    function HttpConnection(url, options) {
        if (options === void 0) { options = {}; }
        this._stopPromiseResolver = function () { };
        this.features = {};
        this._negotiateVersion = 1;
        Utils_1.Arg.isRequired(url, "url");
        this._logger = (0, Utils_1.createLogger)(options.logger);
        this.baseUrl = this._resolveUrl(url);
        options = options || {};
        options.logMessageContent = options.logMessageContent === undefined ? false : options.logMessageContent;
        if (typeof options.withCredentials === "boolean" || options.withCredentials === undefined) {
            options.withCredentials = options.withCredentials === undefined ? true : options.withCredentials;
        }
        else {
            throw new Error("withCredentials option was not a 'boolean' or 'undefined' value");
        }
        options.timeout = options.timeout === undefined ? 100 * 1000 : options.timeout;
        var webSocketModule = null;
        var eventSourceModule = null;
        if (Utils_1.Platform.isNode && typeof require !== "undefined") {
            // In order to ignore the dynamic require in webpack builds we need to do this magic
            // @ts-ignore: TS doesn't know about these names
            var requireFunc = typeof __webpack_require__ === "function" ? __non_webpack_require__ : require;
            webSocketModule = requireFunc("ws");
            eventSourceModule = requireFunc("eventsource");
        }
        if (!Utils_1.Platform.isNode && typeof WebSocket !== "undefined" && !options.WebSocket) {
            options.WebSocket = WebSocket;
        }
        else if (Utils_1.Platform.isNode && !options.WebSocket) {
            if (webSocketModule) {
                options.WebSocket = webSocketModule;
            }
        }
        if (!Utils_1.Platform.isNode && typeof EventSource !== "undefined" && !options.EventSource) {
            options.EventSource = EventSource;
        }
        else if (Utils_1.Platform.isNode && !options.EventSource) {
            if (typeof eventSourceModule !== "undefined") {
                options.EventSource = eventSourceModule;
            }
        }
        this._httpClient = options.httpClient || new DefaultHttpClient_1.DefaultHttpClient(this._logger);
        this._connectionState = "Disconnected" /* ConnectionState.Disconnected */;
        this._connectionStarted = false;
        this._options = options;
        this.onreceive = null;
        this.onclose = null;
    }
    HttpConnection.prototype.start = function (transferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var message, message;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        transferFormat = transferFormat || ITransport_1.TransferFormat.Binary;
                        Utils_1.Arg.isIn(transferFormat, ITransport_1.TransferFormat, "transferFormat");
                        this._logger.log(ILogger_1.LogLevel.Debug, "Starting connection with transfer format '".concat(ITransport_1.TransferFormat[transferFormat], "'."));
                        if (this._connectionState !== "Disconnected" /* ConnectionState.Disconnected */) {
                            return [2 /*return*/, Promise.reject(new Error("Cannot start an HttpConnection that is not in the 'Disconnected' state."))];
                        }
                        this._connectionState = "Connecting" /* ConnectionState.Connecting */;
                        this._startInternalPromise = this._startInternal(transferFormat);
                        return [4 /*yield*/, this._startInternalPromise];
                    case 1:
                        _a.sent();
                        if (!(this._connectionState === "Disconnecting" /* ConnectionState.Disconnecting */)) return [3 /*break*/, 3];
                        message = "Failed to start the HttpConnection before stop() was called.";
                        this._logger.log(ILogger_1.LogLevel.Error, message);
                        // We cannot await stopPromise inside startInternal since stopInternal awaits the startInternalPromise.
                        return [4 /*yield*/, this._stopPromise];
                    case 2:
                        // We cannot await stopPromise inside startInternal since stopInternal awaits the startInternalPromise.
                        _a.sent();
                        return [2 /*return*/, Promise.reject(new Error(message))];
                    case 3:
                        if (this._connectionState !== "Connected" /* ConnectionState.Connected */) {
                            message = "HttpConnection.startInternal completed gracefully but didn't enter the connection into the connected state!";
                            this._logger.log(ILogger_1.LogLevel.Error, message);
                            return [2 /*return*/, Promise.reject(new Error(message))];
                        }
                        _a.label = 4;
                    case 4:
                        this._connectionStarted = true;
                        return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype.send = function (data) {
        if (this._connectionState !== "Connected" /* ConnectionState.Connected */) {
            return Promise.reject(new Error("Cannot send data if the connection is not in the 'Connected' State."));
        }
        if (!this._sendQueue) {
            this._sendQueue = new TransportSendQueue(this.transport);
        }
        // Transport will not be null if state is connected
        return this._sendQueue.send(data);
    };
    HttpConnection.prototype.stop = function (error) {
        return __awaiter(this, void 0, void 0, function () {
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (this._connectionState === "Disconnected" /* ConnectionState.Disconnected */) {
                            this._logger.log(ILogger_1.LogLevel.Debug, "Call to HttpConnection.stop(".concat(error, ") ignored because the connection is already in the disconnected state."));
                            return [2 /*return*/, Promise.resolve()];
                        }
                        if (this._connectionState === "Disconnecting" /* ConnectionState.Disconnecting */) {
                            this._logger.log(ILogger_1.LogLevel.Debug, "Call to HttpConnection.stop(".concat(error, ") ignored because the connection is already in the disconnecting state."));
                            return [2 /*return*/, this._stopPromise];
                        }
                        this._connectionState = "Disconnecting" /* ConnectionState.Disconnecting */;
                        this._stopPromise = new Promise(function (resolve) {
                            // Don't complete stop() until stopConnection() completes.
                            _this._stopPromiseResolver = resolve;
                        });
                        // stopInternal should never throw so just observe it.
                        return [4 /*yield*/, this._stopInternal(error)];
                    case 1:
                        // stopInternal should never throw so just observe it.
                        _a.sent();
                        return [4 /*yield*/, this._stopPromise];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype._stopInternal = function (error) {
        return __awaiter(this, void 0, void 0, function () {
            var e_1, e_2;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        // Set error as soon as possible otherwise there is a race between
                        // the transport closing and providing an error and the error from a close message
                        // We would prefer the close message error.
                        this._stopError = error;
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, , 4]);
                        return [4 /*yield*/, this._startInternalPromise];
                    case 2:
                        _a.sent();
                        return [3 /*break*/, 4];
                    case 3:
                        e_1 = _a.sent();
                        return [3 /*break*/, 4];
                    case 4:
                        if (!this.transport) return [3 /*break*/, 9];
                        _a.label = 5;
                    case 5:
                        _a.trys.push([5, 7, , 8]);
                        return [4 /*yield*/, this.transport.stop()];
                    case 6:
                        _a.sent();
                        return [3 /*break*/, 8];
                    case 7:
                        e_2 = _a.sent();
                        this._logger.log(ILogger_1.LogLevel.Error, "HttpConnection.transport.stop() threw error '".concat(e_2, "'."));
                        this._stopConnection();
                        return [3 /*break*/, 8];
                    case 8:
                        this.transport = undefined;
                        return [3 /*break*/, 10];
                    case 9:
                        this._logger.log(ILogger_1.LogLevel.Debug, "HttpConnection.transport is undefined in HttpConnection.stop() because start() failed.");
                        _a.label = 10;
                    case 10: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype._startInternal = function (transferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var url, negotiateResponse, redirects, _loop_1, this_1, e_3;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        url = this.baseUrl;
                        this._accessTokenFactory = this._options.accessTokenFactory;
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 12, , 13]);
                        if (!this._options.skipNegotiation) return [3 /*break*/, 5];
                        if (!(this._options.transport === ITransport_1.HttpTransportType.WebSockets)) return [3 /*break*/, 3];
                        // No need to add a connection ID in this case
                        this.transport = this._constructTransport(ITransport_1.HttpTransportType.WebSockets);
                        // We should just call connect directly in this case.
                        // No fallback or negotiate in this case.
                        return [4 /*yield*/, this._startTransport(url, transferFormat)];
                    case 2:
                        // We should just call connect directly in this case.
                        // No fallback or negotiate in this case.
                        _a.sent();
                        return [3 /*break*/, 4];
                    case 3: throw new Error("Negotiation can only be skipped when using the WebSocket transport directly.");
                    case 4: return [3 /*break*/, 11];
                    case 5:
                        negotiateResponse = null;
                        redirects = 0;
                        _loop_1 = function () {
                            var accessToken_1;
                            return __generator(this, function (_b) {
                                switch (_b.label) {
                                    case 0: return [4 /*yield*/, this_1._getNegotiationResponse(url)];
                                    case 1:
                                        negotiateResponse = _b.sent();
                                        // the user tries to stop the connection when it is being started
                                        if (this_1._connectionState === "Disconnecting" /* ConnectionState.Disconnecting */ || this_1._connectionState === "Disconnected" /* ConnectionState.Disconnected */) {
                                            throw new Error("The connection was stopped during negotiation.");
                                        }
                                        if (negotiateResponse.error) {
                                            throw new Error(negotiateResponse.error);
                                        }
                                        if (negotiateResponse.ProtocolVersion) {
                                            throw new Error("Detected a connection attempt to an ASP.NET SignalR Server. This client only supports connecting to an ASP.NET Core SignalR Server. See https://aka.ms/signalr-core-differences for details.");
                                        }
                                        if (negotiateResponse.url) {
                                            url = negotiateResponse.url;
                                        }
                                        if (negotiateResponse.accessToken) {
                                            accessToken_1 = negotiateResponse.accessToken;
                                            this_1._accessTokenFactory = function () { return accessToken_1; };
                                        }
                                        redirects++;
                                        return [2 /*return*/];
                                }
                            });
                        };
                        this_1 = this;
                        _a.label = 6;
                    case 6: return [5 /*yield**/, _loop_1()];
                    case 7:
                        _a.sent();
                        _a.label = 8;
                    case 8:
                        if (negotiateResponse.url && redirects < MAX_REDIRECTS) return [3 /*break*/, 6];
                        _a.label = 9;
                    case 9:
                        if (redirects === MAX_REDIRECTS && negotiateResponse.url) {
                            throw new Error("Negotiate redirection limit exceeded.");
                        }
                        return [4 /*yield*/, this._createTransport(url, this._options.transport, negotiateResponse, transferFormat)];
                    case 10:
                        _a.sent();
                        _a.label = 11;
                    case 11:
                        if (this.transport instanceof LongPollingTransport_1.LongPollingTransport) {
                            this.features.inherentKeepAlive = true;
                        }
                        if (this._connectionState === "Connecting" /* ConnectionState.Connecting */) {
                            // Ensure the connection transitions to the connected state prior to completing this.startInternalPromise.
                            // start() will handle the case when stop was called and startInternal exits still in the disconnecting state.
                            this._logger.log(ILogger_1.LogLevel.Debug, "The HttpConnection connected successfully.");
                            this._connectionState = "Connected" /* ConnectionState.Connected */;
                        }
                        return [3 /*break*/, 13];
                    case 12:
                        e_3 = _a.sent();
                        this._logger.log(ILogger_1.LogLevel.Error, "Failed to start the connection: " + e_3);
                        this._connectionState = "Disconnected" /* ConnectionState.Disconnected */;
                        this.transport = undefined;
                        // if start fails, any active calls to stop assume that start will complete the stop promise
                        this._stopPromiseResolver();
                        return [2 /*return*/, Promise.reject(e_3)];
                    case 13: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype._getNegotiationResponse = function (url) {
        return __awaiter(this, void 0, void 0, function () {
            var headers, token, _a, name, value, negotiateUrl, response, negotiateResponse, e_4, errorMessage;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        headers = {};
                        if (!this._accessTokenFactory) return [3 /*break*/, 2];
                        return [4 /*yield*/, this._accessTokenFactory()];
                    case 1:
                        token = _b.sent();
                        if (token) {
                            headers[HeaderNames_1.HeaderNames.Authorization] = "Bearer ".concat(token);
                        }
                        _b.label = 2;
                    case 2:
                        _a = (0, Utils_1.getUserAgentHeader)(), name = _a[0], value = _a[1];
                        headers[name] = value;
                        negotiateUrl = this._resolveNegotiateUrl(url);
                        this._logger.log(ILogger_1.LogLevel.Debug, "Sending negotiation request: ".concat(negotiateUrl, "."));
                        _b.label = 3;
                    case 3:
                        _b.trys.push([3, 5, , 6]);
                        return [4 /*yield*/, this._httpClient.post(negotiateUrl, {
                                content: "",
                                headers: __assign(__assign({}, headers), this._options.headers),
                                timeout: this._options.timeout,
                                withCredentials: this._options.withCredentials,
                            })];
                    case 4:
                        response = _b.sent();
                        if (response.statusCode !== 200) {
                            return [2 /*return*/, Promise.reject(new Error("Unexpected status code returned from negotiate '".concat(response.statusCode, "'")))];
                        }
                        negotiateResponse = JSON.parse(response.content);
                        if (!negotiateResponse.negotiateVersion || negotiateResponse.negotiateVersion < 1) {
                            // Negotiate version 0 doesn't use connectionToken
                            // So we set it equal to connectionId so all our logic can use connectionToken without being aware of the negotiate version
                            negotiateResponse.connectionToken = negotiateResponse.connectionId;
                        }
                        return [2 /*return*/, negotiateResponse];
                    case 5:
                        e_4 = _b.sent();
                        errorMessage = "Failed to complete negotiation with the server: " + e_4;
                        if (e_4 instanceof Errors_1.HttpError) {
                            if (e_4.statusCode === 404) {
                                errorMessage = errorMessage + " Either this is not a SignalR endpoint or there is a proxy blocking the connection.";
                            }
                        }
                        this._logger.log(ILogger_1.LogLevel.Error, errorMessage);
                        return [2 /*return*/, Promise.reject(new Errors_1.FailedToNegotiateWithServerError(errorMessage))];
                    case 6: return [2 /*return*/];
                }
            });
        });
    };
    HttpConnection.prototype._createConnectUrl = function (url, connectionToken) {
        if (!connectionToken) {
            return url;
        }
        return url + (url.indexOf("?") === -1 ? "?" : "&") + "id=".concat(connectionToken);
    };
    HttpConnection.prototype._createTransport = function (url, requestedTransport, negotiateResponse, requestedTransferFormat) {
        return __awaiter(this, void 0, void 0, function () {
            var connectUrl, transportExceptions, transports, negotiate, _i, transports_1, endpoint, transportOrError, ex_1, ex_2, message;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        connectUrl = this._createConnectUrl(url, negotiateResponse.connectionToken);
                        if (!this._isITransport(requestedTransport)) return [3 /*break*/, 2];
                        this._logger.log(ILogger_1.LogLevel.Debug, "Connection was provided an instance of ITransport, using that directly.");
                        this.transport = requestedTransport;
                        return [4 /*yield*/, this._startTransport(connectUrl, requestedTransferFormat)];
                    case 1:
                        _a.sent();
                        this.connectionId = negotiateResponse.connectionId;
                        return [2 /*return*/];
                    case 2:
                        transportExceptions = [];
                        transports = negotiateResponse.availableTransports || [];
                        negotiate = negotiateResponse;
                        _i = 0, transports_1 = transports;
                        _a.label = 3;
                    case 3:
                        if (!(_i < transports_1.length)) return [3 /*break*/, 13];
                        endpoint = transports_1[_i];
                        transportOrError = this._resolveTransportOrError(endpoint, requestedTransport, requestedTransferFormat);
                        if (!(transportOrError instanceof Error)) return [3 /*break*/, 4];
                        // Store the error and continue, we don't want to cause a re-negotiate in these cases
                        transportExceptions.push("".concat(endpoint.transport, " failed:"));
                        transportExceptions.push(transportOrError);
                        return [3 /*break*/, 12];
                    case 4:
                        if (!this._isITransport(transportOrError)) return [3 /*break*/, 12];
                        this.transport = transportOrError;
                        if (!!negotiate) return [3 /*break*/, 9];
                        _a.label = 5;
                    case 5:
                        _a.trys.push([5, 7, , 8]);
                        return [4 /*yield*/, this._getNegotiationResponse(url)];
                    case 6:
                        negotiate = _a.sent();
                        return [3 /*break*/, 8];
                    case 7:
                        ex_1 = _a.sent();
                        return [2 /*return*/, Promise.reject(ex_1)];
                    case 8:
                        connectUrl = this._createConnectUrl(url, negotiate.connectionToken);
                        _a.label = 9;
                    case 9:
                        _a.trys.push([9, 11, , 12]);
                        return [4 /*yield*/, this._startTransport(connectUrl, requestedTransferFormat)];
                    case 10:
                        _a.sent();
                        this.connectionId = negotiate.connectionId;
                        return [2 /*return*/];
                    case 11:
                        ex_2 = _a.sent();
                        this._logger.log(ILogger_1.LogLevel.Error, "Failed to start the transport '".concat(endpoint.transport, "': ").concat(ex_2));
                        negotiate = undefined;
                        transportExceptions.push(new Errors_1.FailedToStartTransportError("".concat(endpoint.transport, " failed: ").concat(ex_2), ITransport_1.HttpTransportType[endpoint.transport]));
                        if (this._connectionState !== "Connecting" /* ConnectionState.Connecting */) {
                            message = "Failed to select transport before stop() was called.";
                            this._logger.log(ILogger_1.LogLevel.Debug, message);
                            return [2 /*return*/, Promise.reject(new Error(message))];
                        }
                        return [3 /*break*/, 12];
                    case 12:
                        _i++;
                        return [3 /*break*/, 3];
                    case 13:
                        if (transportExceptions.length > 0) {
                            return [2 /*return*/, Promise.reject(new Errors_1.AggregateErrors("Unable to connect to the server with any of the available transports. ".concat(transportExceptions.join(" ")), transportExceptions))];
                        }
                        return [2 /*return*/, Promise.reject(new Error("None of the transports supported by the client are supported by the server."))];
                }
            });
        });
    };
    HttpConnection.prototype._constructTransport = function (transport) {
        switch (transport) {
            case ITransport_1.HttpTransportType.WebSockets:
                if (!this._options.WebSocket) {
                    throw new Error("'WebSocket' is not supported in your environment.");
                }
                return new WebSocketTransport_1.WebSocketTransport(this._httpClient, this._accessTokenFactory, this._logger, this._options.logMessageContent, this._options.WebSocket, this._options.headers || {});
            case ITransport_1.HttpTransportType.ServerSentEvents:
                if (!this._options.EventSource) {
                    throw new Error("'EventSource' is not supported in your environment.");
                }
                return new ServerSentEventsTransport_1.ServerSentEventsTransport(this._httpClient, this._accessTokenFactory, this._logger, this._options);
            case ITransport_1.HttpTransportType.LongPolling:
                return new LongPollingTransport_1.LongPollingTransport(this._httpClient, this._accessTokenFactory, this._logger, this._options);
            default:
                throw new Error("Unknown transport: ".concat(transport, "."));
        }
    };
    HttpConnection.prototype._startTransport = function (url, transferFormat) {
        var _this = this;
        this.transport.onreceive = this.onreceive;
        this.transport.onclose = function (e) { return _this._stopConnection(e); };
        return this.transport.connect(url, transferFormat);
    };
    HttpConnection.prototype._resolveTransportOrError = function (endpoint, requestedTransport, requestedTransferFormat) {
        var transport = ITransport_1.HttpTransportType[endpoint.transport];
        if (transport === null || transport === undefined) {
            this._logger.log(ILogger_1.LogLevel.Debug, "Skipping transport '".concat(endpoint.transport, "' because it is not supported by this client."));
            return new Error("Skipping transport '".concat(endpoint.transport, "' because it is not supported by this client."));
        }
        else {
            if (transportMatches(requestedTransport, transport)) {
                var transferFormats = endpoint.transferFormats.map(function (s) { return ITransport_1.TransferFormat[s]; });
                if (transferFormats.indexOf(requestedTransferFormat) >= 0) {
                    if ((transport === ITransport_1.HttpTransportType.WebSockets && !this._options.WebSocket) ||
                        (transport === ITransport_1.HttpTransportType.ServerSentEvents && !this._options.EventSource)) {
                        this._logger.log(ILogger_1.LogLevel.Debug, "Skipping transport '".concat(ITransport_1.HttpTransportType[transport], "' because it is not supported in your environment.'"));
                        return new Errors_1.UnsupportedTransportError("'".concat(ITransport_1.HttpTransportType[transport], "' is not supported in your environment."), transport);
                    }
                    else {
                        this._logger.log(ILogger_1.LogLevel.Debug, "Selecting transport '".concat(ITransport_1.HttpTransportType[transport], "'."));
                        try {
                            return this._constructTransport(transport);
                        }
                        catch (ex) {
                            return ex;
                        }
                    }
                }
                else {
                    this._logger.log(ILogger_1.LogLevel.Debug, "Skipping transport '".concat(ITransport_1.HttpTransportType[transport], "' because it does not support the requested transfer format '").concat(ITransport_1.TransferFormat[requestedTransferFormat], "'."));
                    return new Error("'".concat(ITransport_1.HttpTransportType[transport], "' does not support ").concat(ITransport_1.TransferFormat[requestedTransferFormat], "."));
                }
            }
            else {
                this._logger.log(ILogger_1.LogLevel.Debug, "Skipping transport '".concat(ITransport_1.HttpTransportType[transport], "' because it was disabled by the client."));
                return new Errors_1.DisabledTransportError("'".concat(ITransport_1.HttpTransportType[transport], "' is disabled by the client."), transport);
            }
        }
    };
    HttpConnection.prototype._isITransport = function (transport) {
        return transport && typeof (transport) === "object" && "connect" in transport;
    };
    HttpConnection.prototype._stopConnection = function (error) {
        var _this = this;
        this._logger.log(ILogger_1.LogLevel.Debug, "HttpConnection.stopConnection(".concat(error, ") called while in state ").concat(this._connectionState, "."));
        this.transport = undefined;
        // If we have a stopError, it takes precedence over the error from the transport
        error = this._stopError || error;
        this._stopError = undefined;
        if (this._connectionState === "Disconnected" /* ConnectionState.Disconnected */) {
            this._logger.log(ILogger_1.LogLevel.Debug, "Call to HttpConnection.stopConnection(".concat(error, ") was ignored because the connection is already in the disconnected state."));
            return;
        }
        if (this._connectionState === "Connecting" /* ConnectionState.Connecting */) {
            this._logger.log(ILogger_1.LogLevel.Warning, "Call to HttpConnection.stopConnection(".concat(error, ") was ignored because the connection is still in the connecting state."));
            throw new Error("HttpConnection.stopConnection(".concat(error, ") was called while the connection is still in the connecting state."));
        }
        if (this._connectionState === "Disconnecting" /* ConnectionState.Disconnecting */) {
            // A call to stop() induced this call to stopConnection and needs to be completed.
            // Any stop() awaiters will be scheduled to continue after the onclose callback fires.
            this._stopPromiseResolver();
        }
        if (error) {
            this._logger.log(ILogger_1.LogLevel.Error, "Connection disconnected with error '".concat(error, "'."));
        }
        else {
            this._logger.log(ILogger_1.LogLevel.Information, "Connection disconnected.");
        }
        if (this._sendQueue) {
            this._sendQueue.stop().catch(function (e) {
                _this._logger.log(ILogger_1.LogLevel.Error, "TransportSendQueue.stop() threw error '".concat(e, "'."));
            });
            this._sendQueue = undefined;
        }
        this.connectionId = undefined;
        this._connectionState = "Disconnected" /* ConnectionState.Disconnected */;
        if (this._connectionStarted) {
            this._connectionStarted = false;
            try {
                if (this.onclose) {
                    this.onclose(error);
                }
            }
            catch (e) {
                this._logger.log(ILogger_1.LogLevel.Error, "HttpConnection.onclose(".concat(error, ") threw error '").concat(e, "'."));
            }
        }
    };
    HttpConnection.prototype._resolveUrl = function (url) {
        // startsWith is not supported in IE
        if (url.lastIndexOf("https://", 0) === 0 || url.lastIndexOf("http://", 0) === 0) {
            return url;
        }
        if (!Utils_1.Platform.isBrowser) {
            throw new Error("Cannot resolve '".concat(url, "'."));
        }
        // Setting the url to the href propery of an anchor tag handles normalization
        // for us. There are 3 main cases.
        // 1. Relative path normalization e.g "b" -> "http://localhost:5000/a/b"
        // 2. Absolute path normalization e.g "/a/b" -> "http://localhost:5000/a/b"
        // 3. Networkpath reference normalization e.g "//localhost:5000/a/b" -> "http://localhost:5000/a/b"
        var aTag = window.document.createElement("a");
        aTag.href = url;
        this._logger.log(ILogger_1.LogLevel.Information, "Normalizing '".concat(url, "' to '").concat(aTag.href, "'."));
        return aTag.href;
    };
    HttpConnection.prototype._resolveNegotiateUrl = function (url) {
        var index = url.indexOf("?");
        var negotiateUrl = url.substring(0, index === -1 ? url.length : index);
        if (negotiateUrl[negotiateUrl.length - 1] !== "/") {
            negotiateUrl += "/";
        }
        negotiateUrl += "negotiate";
        negotiateUrl += index === -1 ? "" : url.substring(index);
        if (negotiateUrl.indexOf("negotiateVersion") === -1) {
            negotiateUrl += index === -1 ? "?" : "&";
            negotiateUrl += "negotiateVersion=" + this._negotiateVersion;
        }
        return negotiateUrl;
    };
    return HttpConnection;
}());
exports.HttpConnection = HttpConnection;
function transportMatches(requestedTransport, actualTransport) {
    return !requestedTransport || ((actualTransport & requestedTransport) !== 0);
}
/** @private */
var TransportSendQueue = /** @class */ (function () {
    function TransportSendQueue(_transport) {
        this._transport = _transport;
        this._buffer = [];
        this._executing = true;
        this._sendBufferedData = new PromiseSource();
        this._transportResult = new PromiseSource();
        this._sendLoopPromise = this._sendLoop();
    }
    TransportSendQueue.prototype.send = function (data) {
        this._bufferData(data);
        if (!this._transportResult) {
            this._transportResult = new PromiseSource();
        }
        return this._transportResult.promise;
    };
    TransportSendQueue.prototype.stop = function () {
        this._executing = false;
        this._sendBufferedData.resolve();
        return this._sendLoopPromise;
    };
    TransportSendQueue.prototype._bufferData = function (data) {
        if (this._buffer.length && typeof (this._buffer[0]) !== typeof (data)) {
            throw new Error("Expected data to be of type ".concat(typeof (this._buffer), " but was of type ").concat(typeof (data)));
        }
        this._buffer.push(data);
        this._sendBufferedData.resolve();
    };
    TransportSendQueue.prototype._sendLoop = function () {
        return __awaiter(this, void 0, void 0, function () {
            var transportResult, data, error_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!true) return [3 /*break*/, 6];
                        return [4 /*yield*/, this._sendBufferedData.promise];
                    case 1:
                        _a.sent();
                        if (!this._executing) {
                            if (this._transportResult) {
                                this._transportResult.reject("Connection stopped.");
                            }
                            return [3 /*break*/, 6];
                        }
                        this._sendBufferedData = new PromiseSource();
                        transportResult = this._transportResult;
                        this._transportResult = undefined;
                        data = typeof (this._buffer[0]) === "string" ?
                            this._buffer.join("") :
                            TransportSendQueue._concatBuffers(this._buffer);
                        this._buffer.length = 0;
                        _a.label = 2;
                    case 2:
                        _a.trys.push([2, 4, , 5]);
                        return [4 /*yield*/, this._transport.send(data)];
                    case 3:
                        _a.sent();
                        transportResult.resolve();
                        return [3 /*break*/, 5];
                    case 4:
                        error_1 = _a.sent();
                        transportResult.reject(error_1);
                        return [3 /*break*/, 5];
                    case 5: return [3 /*break*/, 0];
                    case 6: return [2 /*return*/];
                }
            });
        });
    };
    TransportSendQueue._concatBuffers = function (arrayBuffers) {
        var totalLength = arrayBuffers.map(function (b) { return b.byteLength; }).reduce(function (a, b) { return a + b; });
        var result = new Uint8Array(totalLength);
        var offset = 0;
        for (var _i = 0, arrayBuffers_1 = arrayBuffers; _i < arrayBuffers_1.length; _i++) {
            var item = arrayBuffers_1[_i];
            result.set(new Uint8Array(item), offset);
            offset += item.byteLength;
        }
        return result.buffer;
    };
    return TransportSendQueue;
}());
exports.TransportSendQueue = TransportSendQueue;
var PromiseSource = /** @class */ (function () {
    function PromiseSource() {
        var _this = this;
        this.promise = new Promise(function (resolve, reject) {
            var _a;
            return _a = [resolve, reject], _this._resolver = _a[0], _this._rejecter = _a[1], _a;
        });
    }
    PromiseSource.prototype.resolve = function () {
        this._resolver();
    };
    PromiseSource.prototype.reject = function (reason) {
        this._rejecter(reason);
    };
    return PromiseSource;
}());
//# sourceMappingURL=HttpConnection.js.map
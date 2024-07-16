// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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
import { HandshakeProtocol } from "./HandshakeProtocol";
import { AbortError } from "./Errors";
import { MessageType } from "./IHubProtocol";
import { LogLevel } from "./ILogger";
import { Subject } from "./Subject";
import { Arg, getErrorString, Platform } from "./Utils";
import { MessageBuffer } from "./MessageBuffer";
var DEFAULT_TIMEOUT_IN_MS = 30 * 1000;
var DEFAULT_PING_INTERVAL_IN_MS = 15 * 1000;
var DEFAULT_STATEFUL_RECONNECT_BUFFER_SIZE = 100000;
/** Describes the current state of the {@link HubConnection} to the server. */
export var HubConnectionState;
(function (HubConnectionState) {
    /** The hub connection is disconnected. */
    HubConnectionState["Disconnected"] = "Disconnected";
    /** The hub connection is connecting. */
    HubConnectionState["Connecting"] = "Connecting";
    /** The hub connection is connected. */
    HubConnectionState["Connected"] = "Connected";
    /** The hub connection is disconnecting. */
    HubConnectionState["Disconnecting"] = "Disconnecting";
    /** The hub connection is reconnecting. */
    HubConnectionState["Reconnecting"] = "Reconnecting";
})(HubConnectionState || (HubConnectionState = {}));
/** Represents a connection to a SignalR Hub. */
var HubConnection = /** @class */ (function () {
    function HubConnection(connection, logger, protocol, reconnectPolicy, serverTimeoutInMilliseconds, keepAliveIntervalInMilliseconds, statefulReconnectBufferSize) {
        var _this = this;
        this._nextKeepAlive = 0;
        this._freezeEventListener = function () {
            _this._logger.log(LogLevel.Warning, "The page is being frozen, this will likely lead to the connection being closed and messages being lost. For more information see the docs at https://learn.microsoft.com/aspnet/core/signalr/javascript-client#bsleep");
        };
        Arg.isRequired(connection, "connection");
        Arg.isRequired(logger, "logger");
        Arg.isRequired(protocol, "protocol");
        this.serverTimeoutInMilliseconds = serverTimeoutInMilliseconds !== null && serverTimeoutInMilliseconds !== void 0 ? serverTimeoutInMilliseconds : DEFAULT_TIMEOUT_IN_MS;
        this.keepAliveIntervalInMilliseconds = keepAliveIntervalInMilliseconds !== null && keepAliveIntervalInMilliseconds !== void 0 ? keepAliveIntervalInMilliseconds : DEFAULT_PING_INTERVAL_IN_MS;
        this._statefulReconnectBufferSize = statefulReconnectBufferSize !== null && statefulReconnectBufferSize !== void 0 ? statefulReconnectBufferSize : DEFAULT_STATEFUL_RECONNECT_BUFFER_SIZE;
        this._logger = logger;
        this._protocol = protocol;
        this.connection = connection;
        this._reconnectPolicy = reconnectPolicy;
        this._handshakeProtocol = new HandshakeProtocol();
        this.connection.onreceive = function (data) { return _this._processIncomingData(data); };
        this.connection.onclose = function (error) { return _this._connectionClosed(error); };
        this._callbacks = {};
        this._methods = {};
        this._closedCallbacks = [];
        this._reconnectingCallbacks = [];
        this._reconnectedCallbacks = [];
        this._invocationId = 0;
        this._receivedHandshakeResponse = false;
        this._connectionState = HubConnectionState.Disconnected;
        this._connectionStarted = false;
        this._cachedPingMessage = this._protocol.writeMessage({ type: MessageType.Ping });
    }
    /** @internal */
    // Using a public static factory method means we can have a private constructor and an _internal_
    // create method that can be used by HubConnectionBuilder. An "internal" constructor would just
    // be stripped away and the '.d.ts' file would have no constructor, which is interpreted as a
    // public parameter-less constructor.
    HubConnection.create = function (connection, logger, protocol, reconnectPolicy, serverTimeoutInMilliseconds, keepAliveIntervalInMilliseconds, statefulReconnectBufferSize) {
        return new HubConnection(connection, logger, protocol, reconnectPolicy, serverTimeoutInMilliseconds, keepAliveIntervalInMilliseconds, statefulReconnectBufferSize);
    };
    Object.defineProperty(HubConnection.prototype, "state", {
        /** Indicates the state of the {@link HubConnection} to the server. */
        get: function () {
            return this._connectionState;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(HubConnection.prototype, "connectionId", {
        /** Represents the connection id of the {@link HubConnection} on the server. The connection id will be null when the connection is either
         *  in the disconnected state or if the negotiation step was skipped.
         */
        get: function () {
            return this.connection ? (this.connection.connectionId || null) : null;
        },
        enumerable: false,
        configurable: true
    });
    Object.defineProperty(HubConnection.prototype, "baseUrl", {
        /** Indicates the url of the {@link HubConnection} to the server. */
        get: function () {
            return this.connection.baseUrl || "";
        },
        /**
         * Sets a new url for the HubConnection. Note that the url can only be changed when the connection is in either the Disconnected or
         * Reconnecting states.
         * @param {string} url The url to connect to.
         */
        set: function (url) {
            if (this._connectionState !== HubConnectionState.Disconnected && this._connectionState !== HubConnectionState.Reconnecting) {
                throw new Error("The HubConnection must be in the Disconnected or Reconnecting state to change the url.");
            }
            if (!url) {
                throw new Error("The HubConnection url must be a valid url.");
            }
            this.connection.baseUrl = url;
        },
        enumerable: false,
        configurable: true
    });
    /** Starts the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection has been successfully established, or rejects with an error.
     */
    HubConnection.prototype.start = function () {
        this._startPromise = this._startWithStateTransitions();
        return this._startPromise;
    };
    HubConnection.prototype._startWithStateTransitions = function () {
        return __awaiter(this, void 0, void 0, function () {
            var e_1;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (this._connectionState !== HubConnectionState.Disconnected) {
                            return [2 /*return*/, Promise.reject(new Error("Cannot start a HubConnection that is not in the 'Disconnected' state."))];
                        }
                        this._connectionState = HubConnectionState.Connecting;
                        this._logger.log(LogLevel.Debug, "Starting HubConnection.");
                        _a.label = 1;
                    case 1:
                        _a.trys.push([1, 3, , 4]);
                        return [4 /*yield*/, this._startInternal()];
                    case 2:
                        _a.sent();
                        if (Platform.isBrowser) {
                            // Log when the browser freezes the tab so users know why their connection unexpectedly stopped working
                            window.document.addEventListener("freeze", this._freezeEventListener);
                        }
                        this._connectionState = HubConnectionState.Connected;
                        this._connectionStarted = true;
                        this._logger.log(LogLevel.Debug, "HubConnection connected successfully.");
                        return [3 /*break*/, 4];
                    case 3:
                        e_1 = _a.sent();
                        this._connectionState = HubConnectionState.Disconnected;
                        this._logger.log(LogLevel.Debug, "HubConnection failed to start successfully because of error '".concat(e_1, "'."));
                        return [2 /*return*/, Promise.reject(e_1)];
                    case 4: return [2 /*return*/];
                }
            });
        });
    };
    HubConnection.prototype._startInternal = function () {
        return __awaiter(this, void 0, void 0, function () {
            var handshakePromise, version, handshakeRequest, useStatefulReconnect, e_2;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        this._stopDuringStartError = undefined;
                        this._receivedHandshakeResponse = false;
                        handshakePromise = new Promise(function (resolve, reject) {
                            _this._handshakeResolver = resolve;
                            _this._handshakeRejecter = reject;
                        });
                        return [4 /*yield*/, this.connection.start(this._protocol.transferFormat)];
                    case 1:
                        _a.sent();
                        _a.label = 2;
                    case 2:
                        _a.trys.push([2, 7, , 9]);
                        version = this._protocol.version;
                        if (!this.connection.features.reconnect) {
                            // Stateful Reconnect starts with HubProtocol version 2, newer clients connecting to older servers will fail to connect due to
                            // the handshake only supporting version 1, so we will try to send version 1 during the handshake to keep old servers working.
                            version = 1;
                        }
                        handshakeRequest = {
                            protocol: this._protocol.name,
                            version: version,
                        };
                        this._logger.log(LogLevel.Debug, "Sending handshake request.");
                        return [4 /*yield*/, this._sendMessage(this._handshakeProtocol.writeHandshakeRequest(handshakeRequest))];
                    case 3:
                        _a.sent();
                        this._logger.log(LogLevel.Information, "Using HubProtocol '".concat(this._protocol.name, "'."));
                        // defensively cleanup timeout in case we receive a message from the server before we finish start
                        this._cleanupTimeout();
                        this._resetTimeoutPeriod();
                        this._resetKeepAliveInterval();
                        return [4 /*yield*/, handshakePromise];
                    case 4:
                        _a.sent();
                        // It's important to check the stopDuringStartError instead of just relying on the handshakePromise
                        // being rejected on close, because this continuation can run after both the handshake completed successfully
                        // and the connection was closed.
                        if (this._stopDuringStartError) {
                            // It's important to throw instead of returning a rejected promise, because we don't want to allow any state
                            // transitions to occur between now and the calling code observing the exceptions. Returning a rejected promise
                            // will cause the calling continuation to get scheduled to run later.
                            // eslint-disable-next-line @typescript-eslint/no-throw-literal
                            throw this._stopDuringStartError;
                        }
                        useStatefulReconnect = this.connection.features.reconnect || false;
                        if (useStatefulReconnect) {
                            this._messageBuffer = new MessageBuffer(this._protocol, this.connection, this._statefulReconnectBufferSize);
                            this.connection.features.disconnected = this._messageBuffer._disconnected.bind(this._messageBuffer);
                            this.connection.features.resend = function () {
                                if (_this._messageBuffer) {
                                    return _this._messageBuffer._resend();
                                }
                            };
                        }
                        if (!!this.connection.features.inherentKeepAlive) return [3 /*break*/, 6];
                        return [4 /*yield*/, this._sendMessage(this._cachedPingMessage)];
                    case 5:
                        _a.sent();
                        _a.label = 6;
                    case 6: return [3 /*break*/, 9];
                    case 7:
                        e_2 = _a.sent();
                        this._logger.log(LogLevel.Debug, "Hub handshake failed with error '".concat(e_2, "' during start(). Stopping HubConnection."));
                        this._cleanupTimeout();
                        this._cleanupPingTimer();
                        // HttpConnection.stop() should not complete until after the onclose callback is invoked.
                        // This will transition the HubConnection to the disconnected state before HttpConnection.stop() completes.
                        return [4 /*yield*/, this.connection.stop(e_2)];
                    case 8:
                        // HttpConnection.stop() should not complete until after the onclose callback is invoked.
                        // This will transition the HubConnection to the disconnected state before HttpConnection.stop() completes.
                        _a.sent();
                        throw e_2;
                    case 9: return [2 /*return*/];
                }
            });
        });
    };
    /** Stops the connection.
     *
     * @returns {Promise<void>} A Promise that resolves when the connection has been successfully terminated, or rejects with an error.
     */
    HubConnection.prototype.stop = function () {
        return __awaiter(this, void 0, void 0, function () {
            var startPromise, e_3;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        startPromise = this._startPromise;
                        this.connection.features.reconnect = false;
                        this._stopPromise = this._stopInternal();
                        return [4 /*yield*/, this._stopPromise];
                    case 1:
                        _a.sent();
                        _a.label = 2;
                    case 2:
                        _a.trys.push([2, 4, , 5]);
                        // Awaiting undefined continues immediately
                        return [4 /*yield*/, startPromise];
                    case 3:
                        // Awaiting undefined continues immediately
                        _a.sent();
                        return [3 /*break*/, 5];
                    case 4:
                        e_3 = _a.sent();
                        return [3 /*break*/, 5];
                    case 5: return [2 /*return*/];
                }
            });
        });
    };
    HubConnection.prototype._stopInternal = function (error) {
        if (this._connectionState === HubConnectionState.Disconnected) {
            this._logger.log(LogLevel.Debug, "Call to HubConnection.stop(".concat(error, ") ignored because it is already in the disconnected state."));
            return Promise.resolve();
        }
        if (this._connectionState === HubConnectionState.Disconnecting) {
            this._logger.log(LogLevel.Debug, "Call to HttpConnection.stop(".concat(error, ") ignored because the connection is already in the disconnecting state."));
            return this._stopPromise;
        }
        var state = this._connectionState;
        this._connectionState = HubConnectionState.Disconnecting;
        this._logger.log(LogLevel.Debug, "Stopping HubConnection.");
        if (this._reconnectDelayHandle) {
            // We're in a reconnect delay which means the underlying connection is currently already stopped.
            // Just clear the handle to stop the reconnect loop (which no one is waiting on thankfully) and
            // fire the onclose callbacks.
            this._logger.log(LogLevel.Debug, "Connection stopped during reconnect delay. Done reconnecting.");
            clearTimeout(this._reconnectDelayHandle);
            this._reconnectDelayHandle = undefined;
            this._completeClose();
            return Promise.resolve();
        }
        if (state === HubConnectionState.Connected) {
            // eslint-disable-next-line @typescript-eslint/no-floating-promises
            this._sendCloseMessage();
        }
        this._cleanupTimeout();
        this._cleanupPingTimer();
        this._stopDuringStartError = error || new AbortError("The connection was stopped before the hub handshake could complete.");
        // HttpConnection.stop() should not complete until after either HttpConnection.start() fails
        // or the onclose callback is invoked. The onclose callback will transition the HubConnection
        // to the disconnected state if need be before HttpConnection.stop() completes.
        return this.connection.stop(error);
    };
    HubConnection.prototype._sendCloseMessage = function () {
        return __awaiter(this, void 0, void 0, function () {
            var _a;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        _b.trys.push([0, 2, , 3]);
                        return [4 /*yield*/, this._sendWithProtocol(this._createCloseMessage())];
                    case 1:
                        _b.sent();
                        return [3 /*break*/, 3];
                    case 2:
                        _a = _b.sent();
                        return [3 /*break*/, 3];
                    case 3: return [2 /*return*/];
                }
            });
        });
    };
    /** Invokes a streaming hub method on the server using the specified name and arguments.
     *
     * @typeparam T The type of the items returned by the server.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {IStreamResult<T>} An object that yields results from the server as they are received.
     */
    HubConnection.prototype.stream = function (methodName) {
        var _this = this;
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var _a = this._replaceStreamingParams(args), streams = _a[0], streamIds = _a[1];
        var invocationDescriptor = this._createStreamInvocation(methodName, args, streamIds);
        // eslint-disable-next-line prefer-const
        var promiseQueue;
        var subject = new Subject();
        subject.cancelCallback = function () {
            var cancelInvocation = _this._createCancelInvocation(invocationDescriptor.invocationId);
            delete _this._callbacks[invocationDescriptor.invocationId];
            return promiseQueue.then(function () {
                return _this._sendWithProtocol(cancelInvocation);
            });
        };
        this._callbacks[invocationDescriptor.invocationId] = function (invocationEvent, error) {
            if (error) {
                subject.error(error);
                return;
            }
            else if (invocationEvent) {
                // invocationEvent will not be null when an error is not passed to the callback
                if (invocationEvent.type === MessageType.Completion) {
                    if (invocationEvent.error) {
                        subject.error(new Error(invocationEvent.error));
                    }
                    else {
                        subject.complete();
                    }
                }
                else {
                    subject.next((invocationEvent.item));
                }
            }
        };
        promiseQueue = this._sendWithProtocol(invocationDescriptor)
            .catch(function (e) {
            subject.error(e);
            delete _this._callbacks[invocationDescriptor.invocationId];
        });
        this._launchStreams(streams, promiseQueue);
        return subject;
    };
    HubConnection.prototype._sendMessage = function (message) {
        this._resetKeepAliveInterval();
        return this.connection.send(message);
    };
    /**
     * Sends a js object to the server.
     * @param message The js object to serialize and send.
     */
    HubConnection.prototype._sendWithProtocol = function (message) {
        if (this._messageBuffer) {
            return this._messageBuffer._send(message);
        }
        else {
            return this._sendMessage(this._protocol.writeMessage(message));
        }
    };
    /** Invokes a hub method on the server using the specified name and arguments. Does not wait for a response from the receiver.
     *
     * The Promise returned by this method resolves when the client has sent the invocation to the server. The server may still
     * be processing the invocation.
     *
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<void>} A Promise that resolves when the invocation has been successfully sent, or rejects with an error.
     */
    HubConnection.prototype.send = function (methodName) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var _a = this._replaceStreamingParams(args), streams = _a[0], streamIds = _a[1];
        var sendPromise = this._sendWithProtocol(this._createInvocation(methodName, args, true, streamIds));
        this._launchStreams(streams, sendPromise);
        return sendPromise;
    };
    /** Invokes a hub method on the server using the specified name and arguments.
     *
     * The Promise returned by this method resolves when the server indicates it has finished invoking the method. When the promise
     * resolves, the server has finished invoking the method. If the server method returns a result, it is produced as the result of
     * resolving the Promise.
     *
     * @typeparam T The expected return type.
     * @param {string} methodName The name of the server method to invoke.
     * @param {any[]} args The arguments used to invoke the server method.
     * @returns {Promise<T>} A Promise that resolves with the result of the server method (if any), or rejects with an error.
     */
    HubConnection.prototype.invoke = function (methodName) {
        var _this = this;
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        var _a = this._replaceStreamingParams(args), streams = _a[0], streamIds = _a[1];
        var invocationDescriptor = this._createInvocation(methodName, args, false, streamIds);
        var p = new Promise(function (resolve, reject) {
            // invocationId will always have a value for a non-blocking invocation
            _this._callbacks[invocationDescriptor.invocationId] = function (invocationEvent, error) {
                if (error) {
                    reject(error);
                    return;
                }
                else if (invocationEvent) {
                    // invocationEvent will not be null when an error is not passed to the callback
                    if (invocationEvent.type === MessageType.Completion) {
                        if (invocationEvent.error) {
                            reject(new Error(invocationEvent.error));
                        }
                        else {
                            resolve(invocationEvent.result);
                        }
                    }
                    else {
                        reject(new Error("Unexpected message type: ".concat(invocationEvent.type)));
                    }
                }
            };
            var promiseQueue = _this._sendWithProtocol(invocationDescriptor)
                .catch(function (e) {
                reject(e);
                // invocationId will always have a value for a non-blocking invocation
                delete _this._callbacks[invocationDescriptor.invocationId];
            });
            _this._launchStreams(streams, promiseQueue);
        });
        return p;
    };
    HubConnection.prototype.on = function (methodName, newMethod) {
        if (!methodName || !newMethod) {
            return;
        }
        methodName = methodName.toLowerCase();
        if (!this._methods[methodName]) {
            this._methods[methodName] = [];
        }
        // Preventing adding the same handler multiple times.
        if (this._methods[methodName].indexOf(newMethod) !== -1) {
            return;
        }
        this._methods[methodName].push(newMethod);
    };
    HubConnection.prototype.off = function (methodName, method) {
        if (!methodName) {
            return;
        }
        methodName = methodName.toLowerCase();
        var handlers = this._methods[methodName];
        if (!handlers) {
            return;
        }
        if (method) {
            var removeIdx = handlers.indexOf(method);
            if (removeIdx !== -1) {
                handlers.splice(removeIdx, 1);
                if (handlers.length === 0) {
                    delete this._methods[methodName];
                }
            }
        }
        else {
            delete this._methods[methodName];
        }
    };
    /** Registers a handler that will be invoked when the connection is closed.
     *
     * @param {Function} callback The handler that will be invoked when the connection is closed. Optionally receives a single argument containing the error that caused the connection to close (if any).
     */
    HubConnection.prototype.onclose = function (callback) {
        if (callback) {
            this._closedCallbacks.push(callback);
        }
    };
    /** Registers a handler that will be invoked when the connection starts reconnecting.
     *
     * @param {Function} callback The handler that will be invoked when the connection starts reconnecting. Optionally receives a single argument containing the error that caused the connection to start reconnecting (if any).
     */
    HubConnection.prototype.onreconnecting = function (callback) {
        if (callback) {
            this._reconnectingCallbacks.push(callback);
        }
    };
    /** Registers a handler that will be invoked when the connection successfully reconnects.
     *
     * @param {Function} callback The handler that will be invoked when the connection successfully reconnects.
     */
    HubConnection.prototype.onreconnected = function (callback) {
        if (callback) {
            this._reconnectedCallbacks.push(callback);
        }
    };
    HubConnection.prototype._processIncomingData = function (data) {
        var _this = this;
        this._cleanupTimeout();
        if (!this._receivedHandshakeResponse) {
            data = this._processHandshakeResponse(data);
            this._receivedHandshakeResponse = true;
        }
        // Data may have all been read when processing handshake response
        if (data) {
            // Parse the messages
            var messages = this._protocol.parseMessages(data, this._logger);
            for (var _i = 0, messages_1 = messages; _i < messages_1.length; _i++) {
                var message = messages_1[_i];
                if (this._messageBuffer && !this._messageBuffer._shouldProcessMessage(message)) {
                    // Don't process the message, we are either waiting for a SequenceMessage or received a duplicate message
                    continue;
                }
                switch (message.type) {
                    case MessageType.Invocation:
                        this._invokeClientMethod(message)
                            .catch(function (e) {
                            _this._logger.log(LogLevel.Error, "Invoke client method threw error: ".concat(getErrorString(e)));
                        });
                        break;
                    case MessageType.StreamItem:
                    case MessageType.Completion: {
                        var callback = this._callbacks[message.invocationId];
                        if (callback) {
                            if (message.type === MessageType.Completion) {
                                delete this._callbacks[message.invocationId];
                            }
                            try {
                                callback(message);
                            }
                            catch (e) {
                                this._logger.log(LogLevel.Error, "Stream callback threw error: ".concat(getErrorString(e)));
                            }
                        }
                        break;
                    }
                    case MessageType.Ping:
                        // Don't care about pings
                        break;
                    case MessageType.Close: {
                        this._logger.log(LogLevel.Information, "Close message received from server.");
                        var error = message.error ? new Error("Server returned an error on close: " + message.error) : undefined;
                        if (message.allowReconnect === true) {
                            // It feels wrong not to await connection.stop() here, but processIncomingData is called as part of an onreceive callback which is not async,
                            // this is already the behavior for serverTimeout(), and HttpConnection.Stop() should catch and log all possible exceptions.
                            // eslint-disable-next-line @typescript-eslint/no-floating-promises
                            this.connection.stop(error);
                        }
                        else {
                            // We cannot await stopInternal() here, but subsequent calls to stop() will await this if stopInternal() is still ongoing.
                            this._stopPromise = this._stopInternal(error);
                        }
                        break;
                    }
                    case MessageType.Ack:
                        if (this._messageBuffer) {
                            this._messageBuffer._ack(message);
                        }
                        break;
                    case MessageType.Sequence:
                        if (this._messageBuffer) {
                            this._messageBuffer._resetSequence(message);
                        }
                        break;
                    default:
                        this._logger.log(LogLevel.Warning, "Invalid message type: ".concat(message.type, "."));
                        break;
                }
            }
        }
        this._resetTimeoutPeriod();
    };
    HubConnection.prototype._processHandshakeResponse = function (data) {
        var _a;
        var responseMessage;
        var remainingData;
        try {
            _a = this._handshakeProtocol.parseHandshakeResponse(data), remainingData = _a[0], responseMessage = _a[1];
        }
        catch (e) {
            var message = "Error parsing handshake response: " + e;
            this._logger.log(LogLevel.Error, message);
            var error = new Error(message);
            this._handshakeRejecter(error);
            throw error;
        }
        if (responseMessage.error) {
            var message = "Server returned handshake error: " + responseMessage.error;
            this._logger.log(LogLevel.Error, message);
            var error = new Error(message);
            this._handshakeRejecter(error);
            throw error;
        }
        else {
            this._logger.log(LogLevel.Debug, "Server handshake complete.");
        }
        this._handshakeResolver();
        return remainingData;
    };
    HubConnection.prototype._resetKeepAliveInterval = function () {
        if (this.connection.features.inherentKeepAlive) {
            return;
        }
        // Set the time we want the next keep alive to be sent
        // Timer will be setup on next message receive
        this._nextKeepAlive = new Date().getTime() + this.keepAliveIntervalInMilliseconds;
        this._cleanupPingTimer();
    };
    HubConnection.prototype._resetTimeoutPeriod = function () {
        var _this = this;
        if (!this.connection.features || !this.connection.features.inherentKeepAlive) {
            // Set the timeout timer
            this._timeoutHandle = setTimeout(function () { return _this.serverTimeout(); }, this.serverTimeoutInMilliseconds);
            // Set keepAlive timer if there isn't one
            if (this._pingServerHandle === undefined) {
                var nextPing = this._nextKeepAlive - new Date().getTime();
                if (nextPing < 0) {
                    nextPing = 0;
                }
                // The timer needs to be set from a networking callback to avoid Chrome timer throttling from causing timers to run once a minute
                this._pingServerHandle = setTimeout(function () { return __awaiter(_this, void 0, void 0, function () {
                    var _a;
                    return __generator(this, function (_b) {
                        switch (_b.label) {
                            case 0:
                                if (!(this._connectionState === HubConnectionState.Connected)) return [3 /*break*/, 4];
                                _b.label = 1;
                            case 1:
                                _b.trys.push([1, 3, , 4]);
                                return [4 /*yield*/, this._sendMessage(this._cachedPingMessage)];
                            case 2:
                                _b.sent();
                                return [3 /*break*/, 4];
                            case 3:
                                _a = _b.sent();
                                // We don't care about the error. It should be seen elsewhere in the client.
                                // The connection is probably in a bad or closed state now, cleanup the timer so it stops triggering
                                this._cleanupPingTimer();
                                return [3 /*break*/, 4];
                            case 4: return [2 /*return*/];
                        }
                    });
                }); }, nextPing);
            }
        }
    };
    // eslint-disable-next-line @typescript-eslint/naming-convention
    HubConnection.prototype.serverTimeout = function () {
        // The server hasn't talked to us in a while. It doesn't like us anymore ... :(
        // Terminate the connection, but we don't need to wait on the promise. This could trigger reconnecting.
        // eslint-disable-next-line @typescript-eslint/no-floating-promises
        this.connection.stop(new Error("Server timeout elapsed without receiving a message from the server."));
    };
    HubConnection.prototype._invokeClientMethod = function (invocationMessage) {
        return __awaiter(this, void 0, void 0, function () {
            var methodName, methods, methodsCopy, expectsResponse, res, exception, completionMessage, _i, methodsCopy_1, m, prevRes, e_4;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        methodName = invocationMessage.target.toLowerCase();
                        methods = this._methods[methodName];
                        if (!!methods) return [3 /*break*/, 3];
                        this._logger.log(LogLevel.Warning, "No client method with the name '".concat(methodName, "' found."));
                        if (!invocationMessage.invocationId) return [3 /*break*/, 2];
                        this._logger.log(LogLevel.Warning, "No result given for '".concat(methodName, "' method and invocation ID '").concat(invocationMessage.invocationId, "'."));
                        return [4 /*yield*/, this._sendWithProtocol(this._createCompletionMessage(invocationMessage.invocationId, "Client didn't provide a result.", null))];
                    case 1:
                        _a.sent();
                        _a.label = 2;
                    case 2: return [2 /*return*/];
                    case 3:
                        methodsCopy = methods.slice();
                        expectsResponse = invocationMessage.invocationId ? true : false;
                        _i = 0, methodsCopy_1 = methodsCopy;
                        _a.label = 4;
                    case 4:
                        if (!(_i < methodsCopy_1.length)) return [3 /*break*/, 9];
                        m = methodsCopy_1[_i];
                        _a.label = 5;
                    case 5:
                        _a.trys.push([5, 7, , 8]);
                        prevRes = res;
                        return [4 /*yield*/, m.apply(this, invocationMessage.arguments)];
                    case 6:
                        res = _a.sent();
                        if (expectsResponse && res && prevRes) {
                            this._logger.log(LogLevel.Error, "Multiple results provided for '".concat(methodName, "'. Sending error to server."));
                            completionMessage = this._createCompletionMessage(invocationMessage.invocationId, "Client provided multiple results.", null);
                        }
                        // Ignore exception if we got a result after, the exception will be logged
                        exception = undefined;
                        return [3 /*break*/, 8];
                    case 7:
                        e_4 = _a.sent();
                        exception = e_4;
                        this._logger.log(LogLevel.Error, "A callback for the method '".concat(methodName, "' threw error '").concat(e_4, "'."));
                        return [3 /*break*/, 8];
                    case 8:
                        _i++;
                        return [3 /*break*/, 4];
                    case 9:
                        if (!completionMessage) return [3 /*break*/, 11];
                        return [4 /*yield*/, this._sendWithProtocol(completionMessage)];
                    case 10:
                        _a.sent();
                        return [3 /*break*/, 14];
                    case 11:
                        if (!expectsResponse) return [3 /*break*/, 13];
                        // If there is an exception that means either no result was given or a handler after a result threw
                        if (exception) {
                            completionMessage = this._createCompletionMessage(invocationMessage.invocationId, "".concat(exception), null);
                        }
                        else if (res !== undefined) {
                            completionMessage = this._createCompletionMessage(invocationMessage.invocationId, null, res);
                        }
                        else {
                            this._logger.log(LogLevel.Warning, "No result given for '".concat(methodName, "' method and invocation ID '").concat(invocationMessage.invocationId, "'."));
                            // Client didn't provide a result or throw from a handler, server expects a response so we send an error
                            completionMessage = this._createCompletionMessage(invocationMessage.invocationId, "Client didn't provide a result.", null);
                        }
                        return [4 /*yield*/, this._sendWithProtocol(completionMessage)];
                    case 12:
                        _a.sent();
                        return [3 /*break*/, 14];
                    case 13:
                        if (res) {
                            this._logger.log(LogLevel.Error, "Result given for '".concat(methodName, "' method but server is not expecting a result."));
                        }
                        _a.label = 14;
                    case 14: return [2 /*return*/];
                }
            });
        });
    };
    HubConnection.prototype._connectionClosed = function (error) {
        this._logger.log(LogLevel.Debug, "HubConnection.connectionClosed(".concat(error, ") called while in state ").concat(this._connectionState, "."));
        // Triggering this.handshakeRejecter is insufficient because it could already be resolved without the continuation having run yet.
        this._stopDuringStartError = this._stopDuringStartError || error || new AbortError("The underlying connection was closed before the hub handshake could complete.");
        // If the handshake is in progress, start will be waiting for the handshake promise, so we complete it.
        // If it has already completed, this should just noop.
        if (this._handshakeResolver) {
            this._handshakeResolver();
        }
        this._cancelCallbacksWithError(error || new Error("Invocation canceled due to the underlying connection being closed."));
        this._cleanupTimeout();
        this._cleanupPingTimer();
        if (this._connectionState === HubConnectionState.Disconnecting) {
            this._completeClose(error);
        }
        else if (this._connectionState === HubConnectionState.Connected && this._reconnectPolicy) {
            // eslint-disable-next-line @typescript-eslint/no-floating-promises
            this._reconnect(error);
        }
        else if (this._connectionState === HubConnectionState.Connected) {
            this._completeClose(error);
        }
        // If none of the above if conditions were true were called the HubConnection must be in either:
        // 1. The Connecting state in which case the handshakeResolver will complete it and stopDuringStartError will fail it.
        // 2. The Reconnecting state in which case the handshakeResolver will complete it and stopDuringStartError will fail the current reconnect attempt
        //    and potentially continue the reconnect() loop.
        // 3. The Disconnected state in which case we're already done.
    };
    HubConnection.prototype._completeClose = function (error) {
        var _this = this;
        if (this._connectionStarted) {
            this._connectionState = HubConnectionState.Disconnected;
            this._connectionStarted = false;
            if (this._messageBuffer) {
                this._messageBuffer._dispose(error !== null && error !== void 0 ? error : new Error("Connection closed."));
                this._messageBuffer = undefined;
            }
            if (Platform.isBrowser) {
                window.document.removeEventListener("freeze", this._freezeEventListener);
            }
            try {
                this._closedCallbacks.forEach(function (c) { return c.apply(_this, [error]); });
            }
            catch (e) {
                this._logger.log(LogLevel.Error, "An onclose callback called with error '".concat(error, "' threw error '").concat(e, "'."));
            }
        }
    };
    HubConnection.prototype._reconnect = function (error) {
        return __awaiter(this, void 0, void 0, function () {
            var reconnectStartTime, previousReconnectAttempts, retryError, nextRetryDelay, e_5;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        reconnectStartTime = Date.now();
                        previousReconnectAttempts = 0;
                        retryError = error !== undefined ? error : new Error("Attempting to reconnect due to a unknown error.");
                        nextRetryDelay = this._getNextRetryDelay(previousReconnectAttempts++, 0, retryError);
                        if (nextRetryDelay === null) {
                            this._logger.log(LogLevel.Debug, "Connection not reconnecting because the IRetryPolicy returned null on the first reconnect attempt.");
                            this._completeClose(error);
                            return [2 /*return*/];
                        }
                        this._connectionState = HubConnectionState.Reconnecting;
                        if (error) {
                            this._logger.log(LogLevel.Information, "Connection reconnecting because of error '".concat(error, "'."));
                        }
                        else {
                            this._logger.log(LogLevel.Information, "Connection reconnecting.");
                        }
                        if (this._reconnectingCallbacks.length !== 0) {
                            try {
                                this._reconnectingCallbacks.forEach(function (c) { return c.apply(_this, [error]); });
                            }
                            catch (e) {
                                this._logger.log(LogLevel.Error, "An onreconnecting callback called with error '".concat(error, "' threw error '").concat(e, "'."));
                            }
                            // Exit early if an onreconnecting callback called connection.stop().
                            if (this._connectionState !== HubConnectionState.Reconnecting) {
                                this._logger.log(LogLevel.Debug, "Connection left the reconnecting state in onreconnecting callback. Done reconnecting.");
                                return [2 /*return*/];
                            }
                        }
                        _a.label = 1;
                    case 1:
                        if (!(nextRetryDelay !== null)) return [3 /*break*/, 7];
                        this._logger.log(LogLevel.Information, "Reconnect attempt number ".concat(previousReconnectAttempts, " will start in ").concat(nextRetryDelay, " ms."));
                        return [4 /*yield*/, new Promise(function (resolve) {
                                _this._reconnectDelayHandle = setTimeout(resolve, nextRetryDelay);
                            })];
                    case 2:
                        _a.sent();
                        this._reconnectDelayHandle = undefined;
                        if (this._connectionState !== HubConnectionState.Reconnecting) {
                            this._logger.log(LogLevel.Debug, "Connection left the reconnecting state during reconnect delay. Done reconnecting.");
                            return [2 /*return*/];
                        }
                        _a.label = 3;
                    case 3:
                        _a.trys.push([3, 5, , 6]);
                        return [4 /*yield*/, this._startInternal()];
                    case 4:
                        _a.sent();
                        this._connectionState = HubConnectionState.Connected;
                        this._logger.log(LogLevel.Information, "HubConnection reconnected successfully.");
                        if (this._reconnectedCallbacks.length !== 0) {
                            try {
                                this._reconnectedCallbacks.forEach(function (c) { return c.apply(_this, [_this.connection.connectionId]); });
                            }
                            catch (e) {
                                this._logger.log(LogLevel.Error, "An onreconnected callback called with connectionId '".concat(this.connection.connectionId, "; threw error '").concat(e, "'."));
                            }
                        }
                        return [2 /*return*/];
                    case 5:
                        e_5 = _a.sent();
                        this._logger.log(LogLevel.Information, "Reconnect attempt failed because of error '".concat(e_5, "'."));
                        if (this._connectionState !== HubConnectionState.Reconnecting) {
                            this._logger.log(LogLevel.Debug, "Connection moved to the '".concat(this._connectionState, "' from the reconnecting state during reconnect attempt. Done reconnecting."));
                            // The TypeScript compiler thinks that connectionState must be Connected here. The TypeScript compiler is wrong.
                            if (this._connectionState === HubConnectionState.Disconnecting) {
                                this._completeClose();
                            }
                            return [2 /*return*/];
                        }
                        retryError = e_5 instanceof Error ? e_5 : new Error(e_5.toString());
                        nextRetryDelay = this._getNextRetryDelay(previousReconnectAttempts++, Date.now() - reconnectStartTime, retryError);
                        return [3 /*break*/, 6];
                    case 6: return [3 /*break*/, 1];
                    case 7:
                        this._logger.log(LogLevel.Information, "Reconnect retries have been exhausted after ".concat(Date.now() - reconnectStartTime, " ms and ").concat(previousReconnectAttempts, " failed attempts. Connection disconnecting."));
                        this._completeClose();
                        return [2 /*return*/];
                }
            });
        });
    };
    HubConnection.prototype._getNextRetryDelay = function (previousRetryCount, elapsedMilliseconds, retryReason) {
        try {
            return this._reconnectPolicy.nextRetryDelayInMilliseconds({
                elapsedMilliseconds: elapsedMilliseconds,
                previousRetryCount: previousRetryCount,
                retryReason: retryReason,
            });
        }
        catch (e) {
            this._logger.log(LogLevel.Error, "IRetryPolicy.nextRetryDelayInMilliseconds(".concat(previousRetryCount, ", ").concat(elapsedMilliseconds, ") threw error '").concat(e, "'."));
            return null;
        }
    };
    HubConnection.prototype._cancelCallbacksWithError = function (error) {
        var _this = this;
        var callbacks = this._callbacks;
        this._callbacks = {};
        Object.keys(callbacks)
            .forEach(function (key) {
            var callback = callbacks[key];
            try {
                callback(null, error);
            }
            catch (e) {
                _this._logger.log(LogLevel.Error, "Stream 'error' callback called with '".concat(error, "' threw error: ").concat(getErrorString(e)));
            }
        });
    };
    HubConnection.prototype._cleanupPingTimer = function () {
        if (this._pingServerHandle) {
            clearTimeout(this._pingServerHandle);
            this._pingServerHandle = undefined;
        }
    };
    HubConnection.prototype._cleanupTimeout = function () {
        if (this._timeoutHandle) {
            clearTimeout(this._timeoutHandle);
        }
    };
    HubConnection.prototype._createInvocation = function (methodName, args, nonblocking, streamIds) {
        if (nonblocking) {
            if (streamIds.length !== 0) {
                return {
                    arguments: args,
                    streamIds: streamIds,
                    target: methodName,
                    type: MessageType.Invocation,
                };
            }
            else {
                return {
                    arguments: args,
                    target: methodName,
                    type: MessageType.Invocation,
                };
            }
        }
        else {
            var invocationId = this._invocationId;
            this._invocationId++;
            if (streamIds.length !== 0) {
                return {
                    arguments: args,
                    invocationId: invocationId.toString(),
                    streamIds: streamIds,
                    target: methodName,
                    type: MessageType.Invocation,
                };
            }
            else {
                return {
                    arguments: args,
                    invocationId: invocationId.toString(),
                    target: methodName,
                    type: MessageType.Invocation,
                };
            }
        }
    };
    HubConnection.prototype._launchStreams = function (streams, promiseQueue) {
        var _this = this;
        if (streams.length === 0) {
            return;
        }
        // Synchronize stream data so they arrive in-order on the server
        if (!promiseQueue) {
            promiseQueue = Promise.resolve();
        }
        var _loop_1 = function (streamId) {
            streams[streamId].subscribe({
                complete: function () {
                    promiseQueue = promiseQueue.then(function () { return _this._sendWithProtocol(_this._createCompletionMessage(streamId)); });
                },
                error: function (err) {
                    var message;
                    if (err instanceof Error) {
                        message = err.message;
                    }
                    else if (err && err.toString) {
                        message = err.toString();
                    }
                    else {
                        message = "Unknown error";
                    }
                    promiseQueue = promiseQueue.then(function () { return _this._sendWithProtocol(_this._createCompletionMessage(streamId, message)); });
                },
                next: function (item) {
                    promiseQueue = promiseQueue.then(function () { return _this._sendWithProtocol(_this._createStreamItemMessage(streamId, item)); });
                },
            });
        };
        // We want to iterate over the keys, since the keys are the stream ids
        // eslint-disable-next-line guard-for-in
        for (var streamId in streams) {
            _loop_1(streamId);
        }
    };
    HubConnection.prototype._replaceStreamingParams = function (args) {
        var streams = [];
        var streamIds = [];
        for (var i = 0; i < args.length; i++) {
            var argument = args[i];
            if (this._isObservable(argument)) {
                var streamId = this._invocationId;
                this._invocationId++;
                // Store the stream for later use
                streams[streamId] = argument;
                streamIds.push(streamId.toString());
                // remove stream from args
                args.splice(i, 1);
            }
        }
        return [streams, streamIds];
    };
    HubConnection.prototype._isObservable = function (arg) {
        // This allows other stream implementations to just work (like rxjs)
        return arg && arg.subscribe && typeof arg.subscribe === "function";
    };
    HubConnection.prototype._createStreamInvocation = function (methodName, args, streamIds) {
        var invocationId = this._invocationId;
        this._invocationId++;
        if (streamIds.length !== 0) {
            return {
                arguments: args,
                invocationId: invocationId.toString(),
                streamIds: streamIds,
                target: methodName,
                type: MessageType.StreamInvocation,
            };
        }
        else {
            return {
                arguments: args,
                invocationId: invocationId.toString(),
                target: methodName,
                type: MessageType.StreamInvocation,
            };
        }
    };
    HubConnection.prototype._createCancelInvocation = function (id) {
        return {
            invocationId: id,
            type: MessageType.CancelInvocation,
        };
    };
    HubConnection.prototype._createStreamItemMessage = function (id, item) {
        return {
            invocationId: id,
            item: item,
            type: MessageType.StreamItem,
        };
    };
    HubConnection.prototype._createCompletionMessage = function (id, error, result) {
        if (error) {
            return {
                error: error,
                invocationId: id,
                type: MessageType.Completion,
            };
        }
        return {
            invocationId: id,
            result: result,
            type: MessageType.Completion,
        };
    };
    HubConnection.prototype._createCloseMessage = function () {
        return { type: MessageType.Close };
    };
    return HubConnection;
}());
export { HubConnection };

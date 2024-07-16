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
import { MessageType } from "./IHubProtocol";
import { isArrayBuffer } from "./Utils";
/** @private */
var MessageBuffer = /** @class */ (function () {
    function MessageBuffer(protocol, connection, bufferSize) {
        this._bufferSize = 100000;
        this._messages = [];
        this._totalMessageCount = 0;
        this._waitForSequenceMessage = false;
        // Message IDs start at 1 and always increment by 1
        this._nextReceivingSequenceId = 1;
        this._latestReceivedSequenceId = 0;
        this._bufferedByteCount = 0;
        this._reconnectInProgress = false;
        this._protocol = protocol;
        this._connection = connection;
        this._bufferSize = bufferSize;
    }
    MessageBuffer.prototype._send = function (message) {
        return __awaiter(this, void 0, void 0, function () {
            var serializedMessage, backpressurePromise, backpressurePromiseResolver_1, backpressurePromiseRejector_1, _a;
            return __generator(this, function (_b) {
                switch (_b.label) {
                    case 0:
                        serializedMessage = this._protocol.writeMessage(message);
                        backpressurePromise = Promise.resolve();
                        // Only count invocation messages. Acks, pings, etc. don't need to be resent on reconnect
                        if (this._isInvocationMessage(message)) {
                            this._totalMessageCount++;
                            backpressurePromiseResolver_1 = function () { };
                            backpressurePromiseRejector_1 = function () { };
                            if (isArrayBuffer(serializedMessage)) {
                                this._bufferedByteCount += serializedMessage.byteLength;
                            }
                            else {
                                this._bufferedByteCount += serializedMessage.length;
                            }
                            if (this._bufferedByteCount >= this._bufferSize) {
                                backpressurePromise = new Promise(function (resolve, reject) {
                                    backpressurePromiseResolver_1 = resolve;
                                    backpressurePromiseRejector_1 = reject;
                                });
                            }
                            this._messages.push(new BufferedItem(serializedMessage, this._totalMessageCount, backpressurePromiseResolver_1, backpressurePromiseRejector_1));
                        }
                        _b.label = 1;
                    case 1:
                        _b.trys.push([1, 4, , 5]);
                        if (!!this._reconnectInProgress) return [3 /*break*/, 3];
                        return [4 /*yield*/, this._connection.send(serializedMessage)];
                    case 2:
                        _b.sent();
                        _b.label = 3;
                    case 3: return [3 /*break*/, 5];
                    case 4:
                        _a = _b.sent();
                        this._disconnected();
                        return [3 /*break*/, 5];
                    case 5: return [4 /*yield*/, backpressurePromise];
                    case 6:
                        _b.sent();
                        return [2 /*return*/];
                }
            });
        });
    };
    MessageBuffer.prototype._ack = function (ackMessage) {
        var newestAckedMessage = -1;
        // Find index of newest message being acked
        for (var index = 0; index < this._messages.length; index++) {
            var element = this._messages[index];
            if (element._id <= ackMessage.sequenceId) {
                newestAckedMessage = index;
                if (isArrayBuffer(element._message)) {
                    this._bufferedByteCount -= element._message.byteLength;
                }
                else {
                    this._bufferedByteCount -= element._message.length;
                }
                // resolve items that have already been sent and acked
                element._resolver();
            }
            else if (this._bufferedByteCount < this._bufferSize) {
                // resolve items that now fall under the buffer limit but haven't been acked
                element._resolver();
            }
            else {
                break;
            }
        }
        if (newestAckedMessage !== -1) {
            // We're removing everything including the message pointed to, so add 1
            this._messages = this._messages.slice(newestAckedMessage + 1);
        }
    };
    MessageBuffer.prototype._shouldProcessMessage = function (message) {
        if (this._waitForSequenceMessage) {
            if (message.type !== MessageType.Sequence) {
                return false;
            }
            else {
                this._waitForSequenceMessage = false;
                return true;
            }
        }
        // No special processing for acks, pings, etc.
        if (!this._isInvocationMessage(message)) {
            return true;
        }
        var currentId = this._nextReceivingSequenceId;
        this._nextReceivingSequenceId++;
        if (currentId <= this._latestReceivedSequenceId) {
            if (currentId === this._latestReceivedSequenceId) {
                // Should only hit this if we just reconnected and the server is sending
                // Messages it has buffered, which would mean it hasn't seen an Ack for these messages
                this._ackTimer();
            }
            // Ignore, this is a duplicate message
            return false;
        }
        this._latestReceivedSequenceId = currentId;
        // Only start the timer for sending an Ack message when we have a message to ack. This also conveniently solves
        // timer throttling by not having a recursive timer, and by starting the timer via a network call (recv)
        this._ackTimer();
        return true;
    };
    MessageBuffer.prototype._resetSequence = function (message) {
        if (message.sequenceId > this._nextReceivingSequenceId) {
            // eslint-disable-next-line @typescript-eslint/no-floating-promises
            this._connection.stop(new Error("Sequence ID greater than amount of messages we've received."));
            return;
        }
        this._nextReceivingSequenceId = message.sequenceId;
    };
    MessageBuffer.prototype._disconnected = function () {
        this._reconnectInProgress = true;
        this._waitForSequenceMessage = true;
    };
    MessageBuffer.prototype._resend = function () {
        return __awaiter(this, void 0, void 0, function () {
            var sequenceId, messages, _i, messages_1, element;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        sequenceId = this._messages.length !== 0
                            ? this._messages[0]._id
                            : this._totalMessageCount + 1;
                        return [4 /*yield*/, this._connection.send(this._protocol.writeMessage({ type: MessageType.Sequence, sequenceId: sequenceId }))];
                    case 1:
                        _a.sent();
                        messages = this._messages;
                        _i = 0, messages_1 = messages;
                        _a.label = 2;
                    case 2:
                        if (!(_i < messages_1.length)) return [3 /*break*/, 5];
                        element = messages_1[_i];
                        return [4 /*yield*/, this._connection.send(element._message)];
                    case 3:
                        _a.sent();
                        _a.label = 4;
                    case 4:
                        _i++;
                        return [3 /*break*/, 2];
                    case 5:
                        this._reconnectInProgress = false;
                        return [2 /*return*/];
                }
            });
        });
    };
    MessageBuffer.prototype._dispose = function (error) {
        error !== null && error !== void 0 ? error : (error = new Error("Unable to reconnect to server."));
        // Unblock backpressure if any
        for (var _i = 0, _a = this._messages; _i < _a.length; _i++) {
            var element = _a[_i];
            element._rejector(error);
        }
    };
    MessageBuffer.prototype._isInvocationMessage = function (message) {
        // There is no way to check if something implements an interface.
        // So we individually check the messages in a switch statement.
        // To make sure we don't miss any message types we rely on the compiler
        // seeing the function returns a value and it will do the
        // exhaustive check for us on the switch statement, since we don't use 'case default'
        switch (message.type) {
            case MessageType.Invocation:
            case MessageType.StreamItem:
            case MessageType.Completion:
            case MessageType.StreamInvocation:
            case MessageType.CancelInvocation:
                return true;
            case MessageType.Close:
            case MessageType.Sequence:
            case MessageType.Ping:
            case MessageType.Ack:
                return false;
        }
    };
    MessageBuffer.prototype._ackTimer = function () {
        var _this = this;
        if (this._ackTimerHandle === undefined) {
            this._ackTimerHandle = setTimeout(function () { return __awaiter(_this, void 0, void 0, function () {
                var _a;
                return __generator(this, function (_b) {
                    switch (_b.label) {
                        case 0:
                            _b.trys.push([0, 3, , 4]);
                            if (!!this._reconnectInProgress) return [3 /*break*/, 2];
                            return [4 /*yield*/, this._connection.send(this._protocol.writeMessage({ type: MessageType.Ack, sequenceId: this._latestReceivedSequenceId }))];
                        case 1:
                            _b.sent();
                            _b.label = 2;
                        case 2: return [3 /*break*/, 4];
                        case 3:
                            _a = _b.sent();
                            return [3 /*break*/, 4];
                        case 4:
                            clearTimeout(this._ackTimerHandle);
                            this._ackTimerHandle = undefined;
                            return [2 /*return*/];
                    }
                });
            }); }, 1000);
        }
    };
    return MessageBuffer;
}());
export { MessageBuffer };
var BufferedItem = /** @class */ (function () {
    function BufferedItem(message, id, resolver, rejector) {
        this._message = message;
        this._id = id;
        this._resolver = resolver;
        this._rejector = rejector;
    }
    return BufferedItem;
}());

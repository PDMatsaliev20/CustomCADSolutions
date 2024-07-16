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
import { HeaderNames } from "./HeaderNames";
import { HttpClient } from "./HttpClient";
/** @private */
var AccessTokenHttpClient = /** @class */ (function (_super) {
    __extends(AccessTokenHttpClient, _super);
    function AccessTokenHttpClient(innerClient, accessTokenFactory) {
        var _this = _super.call(this) || this;
        _this._innerClient = innerClient;
        _this._accessTokenFactory = accessTokenFactory;
        return _this;
    }
    AccessTokenHttpClient.prototype.send = function (request) {
        return __awaiter(this, void 0, void 0, function () {
            var allowRetry, _a, response, _b;
            return __generator(this, function (_c) {
                switch (_c.label) {
                    case 0:
                        allowRetry = true;
                        if (!(this._accessTokenFactory && (!this._accessToken || (request.url && request.url.indexOf("/negotiate?") > 0)))) return [3 /*break*/, 2];
                        // don't retry if the request is a negotiate or if we just got a potentially new token from the access token factory
                        allowRetry = false;
                        _a = this;
                        return [4 /*yield*/, this._accessTokenFactory()];
                    case 1:
                        _a._accessToken = _c.sent();
                        _c.label = 2;
                    case 2:
                        this._setAuthorizationHeader(request);
                        return [4 /*yield*/, this._innerClient.send(request)];
                    case 3:
                        response = _c.sent();
                        if (!(allowRetry && response.statusCode === 401 && this._accessTokenFactory)) return [3 /*break*/, 6];
                        _b = this;
                        return [4 /*yield*/, this._accessTokenFactory()];
                    case 4:
                        _b._accessToken = _c.sent();
                        this._setAuthorizationHeader(request);
                        return [4 /*yield*/, this._innerClient.send(request)];
                    case 5: return [2 /*return*/, _c.sent()];
                    case 6: return [2 /*return*/, response];
                }
            });
        });
    };
    AccessTokenHttpClient.prototype._setAuthorizationHeader = function (request) {
        if (!request.headers) {
            request.headers = {};
        }
        if (this._accessToken) {
            request.headers[HeaderNames.Authorization] = "Bearer ".concat(this._accessToken);
        }
        // don't remove the header if there isn't an access token factory, the user manually added the header in this case
        else if (this._accessTokenFactory) {
            if (request.headers[HeaderNames.Authorization]) {
                delete request.headers[HeaderNames.Authorization];
            }
        }
    };
    AccessTokenHttpClient.prototype.getCookieString = function (url) {
        return this._innerClient.getCookieString(url);
    };
    return AccessTokenHttpClient;
}(HttpClient));
export { AccessTokenHttpClient };

// package: 
// file: convert.proto

var convert_pb = require("./convert_pb");
var grpc = require("@improbable-eng/grpc-web").grpc;

var GrpcConvert = (function () {
  function GrpcConvert() {}
  GrpcConvert.serviceName = "GrpcConvert";
  return GrpcConvert;
}());

GrpcConvert.Convert = {
  methodName: "Convert",
  service: GrpcConvert,
  requestStream: false,
  responseStream: false,
  requestType: convert_pb.ConvertRequest,
  responseType: convert_pb.ConvertResponse
};

exports.GrpcConvert = GrpcConvert;

function GrpcConvertClient(serviceHost, options) {
  this.serviceHost = serviceHost;
  this.options = options || {};
}

GrpcConvertClient.prototype.convert = function convert(requestMessage, metadata, callback) {
  if (arguments.length === 2) {
    callback = arguments[1];
  }
  var client = grpc.unary(GrpcConvert.Convert, {
    request: requestMessage,
    host: this.serviceHost,
    metadata: metadata,
    transport: this.options.transport,
    debug: this.options.debug,
    onEnd: function (response) {
      if (callback) {
        if (response.status !== grpc.Code.OK) {
          var err = new Error(response.statusMessage);
          err.code = response.status;
          err.metadata = response.trailers;
          callback(err, null);
        } else {
          callback(null, response.message);
        }
      }
    }
  });
  return {
    cancel: function () {
      callback = null;
      client.close();
    }
  };
};

exports.GrpcConvertClient = GrpcConvertClient;


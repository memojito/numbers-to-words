// package: 
// file: convert.proto

import * as convert_pb from "./convert_pb";
import {grpc} from "@improbable-eng/grpc-web";

type GrpcConvertConvert = {
  readonly methodName: string;
  readonly service: typeof GrpcConvert;
  readonly requestStream: false;
  readonly responseStream: false;
  readonly requestType: typeof convert_pb.ConvertRequest;
  readonly responseType: typeof convert_pb.ConvertResponse;
};

export class GrpcConvert {
  static readonly serviceName: string;
  static readonly Convert: GrpcConvertConvert;
}

export type ServiceError = { message: string, code: number; metadata: grpc.Metadata }
export type Status = { details: string, code: number; metadata: grpc.Metadata }

interface UnaryResponse {
  cancel(): void;
}
interface ResponseStream<T> {
  cancel(): void;
  on(type: 'data', handler: (message: T) => void): ResponseStream<T>;
  on(type: 'end', handler: (status?: Status) => void): ResponseStream<T>;
  on(type: 'status', handler: (status: Status) => void): ResponseStream<T>;
}
interface RequestStream<T> {
  write(message: T): RequestStream<T>;
  end(): void;
  cancel(): void;
  on(type: 'end', handler: (status?: Status) => void): RequestStream<T>;
  on(type: 'status', handler: (status: Status) => void): RequestStream<T>;
}
interface BidirectionalStream<ReqT, ResT> {
  write(message: ReqT): BidirectionalStream<ReqT, ResT>;
  end(): void;
  cancel(): void;
  on(type: 'data', handler: (message: ResT) => void): BidirectionalStream<ReqT, ResT>;
  on(type: 'end', handler: (status?: Status) => void): BidirectionalStream<ReqT, ResT>;
  on(type: 'status', handler: (status: Status) => void): BidirectionalStream<ReqT, ResT>;
}

export class GrpcConvertClient {
  readonly serviceHost: string;

  constructor(serviceHost: string, options?: grpc.RpcOptions);
  convert(
    requestMessage: convert_pb.ConvertRequest,
    metadata: grpc.Metadata,
    callback: (error: ServiceError|null, responseMessage: convert_pb.ConvertResponse|null) => void
  ): UnaryResponse;
  convert(
    requestMessage: convert_pb.ConvertRequest,
    callback: (error: ServiceError|null, responseMessage: convert_pb.ConvertResponse|null) => void
  ): UnaryResponse;
}


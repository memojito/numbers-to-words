// package: 
// file: convert.proto

import * as jspb from "google-protobuf";

export class ConvertRequest extends jspb.Message {
  getInput(): number;
  setInput(value: number): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ConvertRequest.AsObject;
  static toObject(includeInstance: boolean, msg: ConvertRequest): ConvertRequest.AsObject;
  static extensions: {[key: number]: jspb.ExtensionFieldInfo<jspb.Message>};
  static extensionsBinary: {[key: number]: jspb.ExtensionFieldBinaryInfo<jspb.Message>};
  static serializeBinaryToWriter(message: ConvertRequest, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ConvertRequest;
  static deserializeBinaryFromReader(message: ConvertRequest, reader: jspb.BinaryReader): ConvertRequest;
}

export namespace ConvertRequest {
  export type AsObject = {
    input: number,
  }
}

export class ConvertResponse extends jspb.Message {
  getOutput(): string;
  setOutput(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): ConvertResponse.AsObject;
  static toObject(includeInstance: boolean, msg: ConvertResponse): ConvertResponse.AsObject;
  static extensions: {[key: number]: jspb.ExtensionFieldInfo<jspb.Message>};
  static extensionsBinary: {[key: number]: jspb.ExtensionFieldBinaryInfo<jspb.Message>};
  static serializeBinaryToWriter(message: ConvertResponse, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): ConvertResponse;
  static deserializeBinaryFromReader(message: ConvertResponse, reader: jspb.BinaryReader): ConvertResponse;
}

export namespace ConvertResponse {
  export type AsObject = {
    output: string,
  }
}


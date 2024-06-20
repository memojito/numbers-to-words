<template>
  <div class="home">
    <div class="input-field">
      <input id="inputField" type="text" v-model="inputText" placeholder="Enter a number here!" class="input-text">
      <div class="validation-message">{{ validationMessage }}</div>
    </div>
    <div class="output-field">
      <div id="outputField" class="output-text">{{ outputText }}</div>
    </div>
  </div>
</template>

<style scoped>
.home {
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 100vh;
  padding: 0 10%;
}

.input-field,
.output-field {
  width: 40%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: flex-start;
  height: 100%;
}

.input-text {
  font-size: 1.2em;
}

.output-text {
  font-size: 1.2em; 
}

.validation-message {
  color: red;
  font-size: 0.8em;
  margin-top: 5px;
  text-align: left;
}
</style>

<script>
import { grpc } from "@improbable-eng/grpc-web";
import { ConvertRequest } from '@/gen/convert_pb'; 
import { GrpcConvert } from '@/gen/convert_pb_service';
export default {
  name: 'HomeView',
  data() {
    return {
      inputText: '',
      outputText: '',
      validationMessage: ''
    };
  },
  watch: {
    // call server every time there is new input
    inputText(newInput, oldInput) {
      if (newInput !== oldInput) {
        this.validateInput();
      }
    }
  },
  methods: {
    validateInput() {
      const input = this.inputText.trim();

      // handle both comma and dot
      let cleanedInput = input.replace(",", ".");

      // check if positive
      if (!/^\d*\.?\d+$/.test(cleanedInput)) {
        this.validationMessage = 'Please enter a positive number.';
        this.outputText = '';
      } else {
        // check if no more than 2 digits after comma
        let number = parseFloat(cleanedInput);
        if (number < 0) {
          this.validationMessage = 'Please enter a positive number.';
          this.outputText = '';
        } else if (number.toString().split('.')[1]?.length > 2) {
          this.validationMessage = 'Please enter only two digits after comma.';
          this.outputText = '';
        } else {
          this.validationMessage = '';
          this.convert(cleanedInput);
        }
      }
    },
    convert(cleaned) {
      console.log('Convert called with input:', cleaned);

      // check if inputText is empty
      if (cleaned.trim() === '') {
        this.outputText = ''; 
        return;
      }

      const request = new ConvertRequest();
      request.setInput(cleaned);

      grpc.unary(GrpcConvert.Convert, {
        request: request,
        host: "http://localhost:5001",
        onEnd: (response) => {
            const { status, message } = response;

            if (status === grpc.Code.OK && message) {
                const outputValue = message.getOutput();
                this.outputText = outputValue;
            } else {
                console.error('Error:', status, message);
            }
        }
      });
    }
  }
};
</script>

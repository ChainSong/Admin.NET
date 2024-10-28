<template>
    <input type="text" @input="$emit('update:modelValue', $event.target.value)" :value="modelValue" />
  </template>
   
  <script>
  export default {
    name: 'tableModel',
    props: {
      modelValue: String
    }
  };
  </script>
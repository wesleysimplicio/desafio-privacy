<template>
  <!-- <img alt="Vue logo" src="./assets/logo.png"> -->
  <DeliveryComponent />
</template>

<script>
import DeliveryComponent from './components/DeliveryComponent.vue';

export default {
  name: 'App',
  components: {
    DeliveryComponent
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>

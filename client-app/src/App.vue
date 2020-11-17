<template>
  <div id="app">
    <router-view/>
  </div>
</template>

// <script>
/* eslint-disable */
import axios from 'axios'

export default {
  created: function () {
    axios.interceptors.response.use(undefined, function (err) {
      return new Promise(function (resolve, reject) {
        if (err.status === 401 && err.config && !err.config.__isRetryRequest) {
          this.$store.dispatch(logout)
        }
        throw err;
      });
    });
  }
}
</script>
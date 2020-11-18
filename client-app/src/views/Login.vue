<template>
  <div id="app">
    <v-app>
      <v-dialog v-model="dialog" persistent max-width="600px" min-width="360px">
        <div>
          <v-card class="px-4">
            <v-card-text>
              <v-form>
                <v-row>
                  <v-col cols="12">
                    <v-text-field
                      v-model="password"
                      :append-icon="show ? 'eye' : 'eye-off'"
                      :type="show ? 'text' : 'password'"
                      name="input-10-1"
                      label="Пароль"
                      counter
                      @click:append="show = !show"
                      @keydown.enter="login"
                    ></v-text-field>
                  </v-col>
                  <v-col class="d-flex" cols="12" sm="6" xsm="12"> </v-col>
                  <v-spacer></v-spacer>
                  <v-col class="d-flex" cols="12" sm="3" xsm="12" align-end>
                    <v-btn
                      x-large
                      block
                      :disabled="!password"
                      color="success"
                      @click="login"
                    >
                      Войти
                    </v-btn>
                  </v-col>
                </v-row>
              </v-form>
            </v-card-text>
          </v-card>
        </div>
      </v-dialog>
    </v-app>
  </div>
</template>

<script>
export default {
  data() {
    return {
      password: "",
      show: false,
      dialog: true,
    };
  },
  methods: {
    login: function () {
      let password = this.password;
      this.$store
        .dispatch("login", { password })
        .then(() => {
            if (this.$store.getters.isLoggedIn) {
              this.$router.push({ path: "/" });
          }
        })
        .catch((err) => console.log(err));
    },
  },
};
</script>
import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

const APIurl = '/api';

export default new Vuex.Store({
  state: {
    carPartItems: [],
    status: '',
    token: localStorage.getItem('token') || '',
    user: {}
  },
  mutations: {
    setParts(state, data) {
      state.carPartItems = data.itemsOnPage.map(x => x);
    },
    authSuccess(state, token, user) {
      state.status = 'success';
      state.token = token;
      state.user = user;
    },
    authError(state) {
      state.status = 'error'
    },
    logout(state) {
      state.status = ''
      state.token = ''
    },
  },
  actions: {
    getPartsByPageNumber: ({ commit }, searchParams) => {
      axios.get(`${APIurl}/CarParts/GetPartsByPageNumber/`, {
        params: {
          pageIndex: searchParams.page,
          sectionIndex: searchParams.sectionIndex,
          searchWord: searchParams.searchWord,
        },
      })
        .then(response => {
          commit('setParts', response.data);
        })
        .catch(e => {
          console.log(e);
        })
    },
    /* eslint-disable no-unused-vars */
    editPartById: ({ commit }, id) => {
      axios.get(`${APIurl}/CarParts/GetPartsByPageNumber/`, {
        params: {
        }
      })
        .then(() => {
        })
        .catch(e => {
          console.log(e);
        })
    },
    /* eslint-enable no-unused-vars */
    login: ({ commit }, user) => {
      axios.post(`${APIurl}/Users/Login/`, {
        password: user.password
      }).then((response) => {
        const token = `Bearer ${response.data.access_token}`;
        const user = 'response.data.user';
        localStorage.setItem('token', token)
        axios.defaults.headers.common['Authorization'] = token;
        commit('authSuccess', token, user)
      }).catch((e) => {
        commit('authError');
        localStorage.removeItem('token');
        console.log(e);
      })
    },
    logout: ({ commit }) => {
      commit('logout')
      localStorage.removeItem('token')
      delete axios.defaults.headers.common['Authorization']
    }
  },
  modules: {
  },
  getters: {
    carPartItems: (state) => () => {
      return state.carPartItems;
    },
    isLoggedIn: state => state.status === 'success',
    authStatus: state => state.status,
  }
})

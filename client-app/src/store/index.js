import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        carPartItems: [],
        carPartItemsLength: 0,
        status: '',
        token: localStorage.getItem('token') || '',
        user: {}
    },
    mutations: {
        setParts(state, data) {
            state.carPartItems = data.itemsOnPage.map(x => x);
            state.carPartItemsLength = Math.ceil(data.totalItems / 15);
        },
        removePart(state, id) {
            state.carPartItems = state.carPartItems.filter(p => p.id !== id);
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
            axios.get(`api/CarParts/GetPartsByPageNumber/`, {
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
        updatePart: ({ commit }, partToUpdate) => {
            console.log(partToUpdate.id)
            axios.put(`api/CarParts/UpdatePart/`, {
                id: partToUpdate.id,
                partName: partToUpdate.partName,
                note: partToUpdate.note,
                unitOfMeasure: partToUpdate.unitOfMeasure,
                volume: partToUpdate.volume,
                weigth: partToUpdate.weigth,
                availibility: partToUpdate.availibility,
                cost: partToUpdate.cost,
                oem: partToUpdate.oem,
                section: partToUpdate.section,
                subsection: partToUpdate.subsection,
                costNumber: partToUpdate.costNumber,
                picSrc: partToUpdate.picSrc,
                partCode: partToUpdate.partCode,
                sectionAndSubsectionId: partToUpdate.sectionAndSubsectionId,
            })
                .then((response) => {
                    console.log(response.status);
                })
                .catch(e => {
                    console.log(e);
                })
        },
        createPart: ({ commit }, partToCreate) => {
            console.log(partToCreate)
            axios.post(`api/CarParts/CreatePart/`, {
                    partName: partToCreate.partName,
                    note: partToCreate.note,
                    unitOfMeasure: partToCreate.unitOfMeasure,
                    volume: partToCreate.volume,
                    weigth: partToCreate.weigth,
                    availibility: partToCreate.availibility,
                    cost: partToCreate.cost,
                    oem: partToCreate.oem,
                    section: partToCreate.section,
                    subsection: partToCreate.subsection,
                    costNumber: partToCreate.costNumber,
                    picSrc: partToCreate.picSrc,
                    partCode: partToCreate.partCode,
                    sectionAndSubsectionId: partToCreate.sectionAndSubsectionId,
            })
                .then((response) => console.log(response.status))
                .catch(e => console.log(e));
        },
        deletePartById: ({ commit }, param) => {
            axios.delete(`api/CarParts/DeletePartById/?id=${param.id}`).then((response) => {
                console.log(response.status);
                commit('removePart', param.id);
            }).catch((e) => console.log(e));
        },
        /* eslint-enable no-unused-vars */
        login: ({ commit }, user) => {
            axios.post(`api/Users/Login/`, {
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
        carPartItemsLength: state => state.carPartItemsLength,
        isLoggedIn: state => state.status === 'success',
        authStatus: state => state.status,
    }
})

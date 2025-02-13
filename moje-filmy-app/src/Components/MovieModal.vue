<script setup>
import { ref, onMounted, onUnmounted, computed, reactive} from "vue";
import { useVuelidate } from '@vuelidate/core'
import { required, maxLength, between, helpers  } from '@vuelidate/validators'
import { Modal } from "bootstrap";
import { eventBus } from "../eventBus";
import MovieService from "../services/MovieService";

let movieModalRef = ref(null);
let modalInstance = null;
let mode = ref(null);

const movie = reactive({
    title: null,
    director: null,
    year: null,
    rate: null
})

const form = ref(movie);

let errorMessage = ref(null);

const resetMovie = () => {
    movie.title = null;
    movie.director = null;
    movie.year = null;
    movie.rate = null;
}

onMounted(() => {
    if (movieModalRef.value) {
        modalInstance = new Modal(movieModalRef.value);
    }
    eventBus.on("openMovieModal", (data) => {
        mode.value = data.mode
        if (mode.value === "add") {
            resetMovie();
        }
        else {
            Object.assign(movie, data.movie);
            console.log(movie);
        }
        modalInstance?.show();
    });
});

onUnmounted(() => {
    eventBus.off("openMovieModal");
});

const addMovie = async () => {
    console.log('add movie ' + movie.title);
    var response = await MovieService.addMovie(movie);
    if (response.success === true) {
        errorMessage = null;
        eventBus.emit("movieAdded", response.data.id);
    }
    else {
        errorMessage = response.errorMessage;
        console.log(response);
    }
};

const editMovie = async () => {
    console.log('edit movie ' + movie.title);
    var response = await MovieService.editMovie(movie);
    if (response.success === true) {
        errorMessage = null;
        eventBus.emit("movieEdited", movie.id);
    }
    else {
        errorMessage = response.errorMessage;
        console.log(response);
    }
};

const rules = computed(() => {
    return {
        title: { 
            required: helpers.withMessage("To pole jest obowiązkowe", required), 
            maxLength: helpers.withMessage("Tytuł może mieć maksymalnie 200 znaków", maxLength(200)), 
            $lazy: true },
        director: { },
        year: { 
            between: helpers.withMessage("Rok musi być pomiędzy 1900 a 2200", between(1900, 2200)), $lazy: true },
        rate: { }
    }
})

const v$ = useVuelidate(rules, form)

const handleSubmit = async () => {
    const validation = await v$.value.$validate();
    if (!validation) {
        return;
    }
    if (mode.value === "add") {
        addMovie();
    }
    else {
        editMovie();
    }
    closeModal();
    resetMovie();
};

const closeModal = () => {
    errorMessage = null;
    v$.value.$reset();
    modalInstance?.hide();
};

</script>

<template>
    <div class="modal fade" ref="movieModalRef" id="deleteModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="deleteModalLabel">{{ mode === "add" ? "Dodaj nowy film" : "Edytuj film" }}</h1>
                    <button type="button" class="btn-close" @click="closeModal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent="handleSubmit">
                        <div class="mb-3">
                            <label for="movieTitleInput" class="form-label">Tytuł</label>
                            <input type="text" class="form-control" id="movieTitleInput" v-model="form.title">
                            <div v-if="v$.title.$error" class="error-message">
                                <p>{{ v$.title.$errors[0].$message }}</p>
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="movieDirectorInput" class="form-label">Reżyser</label>
                            <input type="text" class="form-control" id="movieDirectorInput" v-model="form.director">
                        </div>
                        <div class="mb-3">
                            <label for="movieYearInput" class="form-label">Rok</label>
                            <input type="number" class="form-control" id="movieYearInput" v-model="form.year">
                            <div v-if="v$.year.$error" class="error-message">
                                <p>{{ v$.year.$errors[0].$message }}</p>
                            </div>
                        </div>
                        <div class="mb-3">
                            <label for="movieRateInput" class="form-label">Ocena</label>
                            <input type="number" class="form-control" id="movieRateInput" v-model="form.rate">
                        </div>
                    </form>
                </div>
                <div v-if="errorMessage" class="alert alert-danger error-alert" role="alert">
                    <p>Wystąpił błąd: </p>
                    {{ errorMessage }}
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" @click="closeModal">Anuluj</button>
                    <button type="submit" class="btn btn-primary" @click="handleSubmit">Zapisz zmiany</button>
                </div>
            </div>
        </div>
    </div>
</template>

<style>
.error-alert {
    margin: 10px;
}
</style>
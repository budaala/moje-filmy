// import movieData from "../data.json";
import axios from "axios";

const axiosInstance = axios.create({
  baseURL: "https://localhost:7052/api/Movie",
  headers: {
    "Content-Type": "application/json",
  },
});

// getting movies from database
const getMovies = async () => {
  try {
    var response = await axiosInstance.get("/getMovies");
    if (response.status === 200) {
      if (response.data === null) {
        return {
          success: false,
          data: [],
          errorMessage: "Nie znaleziono filmów",
        };
      }
      return { success: true, data: response.data };
    } else {
      return {
        success: false,
        data: [],
        errorMessage: "Nie udało się pobrać filmów",
      };
    }
  } catch (error) {
    console.log(error);
    return {
      success: false,
      data: [],
      errorMessage: "Nie udało się pobrać filmów. Błąd połączenia z serwerem.",
    };
  }
};

// getting movies from outside api and adding to database
const getNewMovies = async () => {
  try {
    var response = await axiosInstance.get("/getNewMovies");
    if (response.status === 200) {
      if (response.data === null || response.data.length === 0) {
        return {
          success: false,
          data: [],
          errorMessage: "Nie znaleziono nowych filmów",
        };
      }
      return { success: true, data: response.data, successMessage: returnSuccessMessage(response.data.length) };
    } else {
      return {
        success: false,
        data: [],
        errorMessage: "Nie udało się pobrać nowych filmów",
      };
    }
  } catch (error) {
    console.log(error);
    return {
      success: false,
      data: [],
      errorMessage:
        "Nie udało się pobrać nowych filmów. Błąd połączenia z serwerem.",
    };
  }
};

const returnSuccessMessage = (movieCount) => {
  if (movieCount === 1) {
    return "Pobrano 1 nowy film";
  }
  if (movieCount > 1 && movieCount < 5) {
    return "Pobrano " + movieCount + " nowe filmy";
  }
  return "Pobrano " + movieCount + " nowych filmów";
}

const addMovie = async (movie) => {
  try {
    var response = await axiosInstance.post("/addMovie", movie);
    if (response.status === 200) {
      return { success: true, data: response.data };
    } else {
      return { success: false, errorMessage: "Nie udało się dodać filmu" };
    }
  } catch (error) {
    console.log(error);
    return {
      success: false,
      errorMessage: "Nie udało się dodać filmu. Błąd połączenia z serwerem.",
    };
  }
};

const editMovie = async (movie) => {
  try {
    var response = await axiosInstance.put("/editMovie", movie);
    if (response.status === 200) {
      return { success: true, data: response.data };
    } else {
      return { success: false, errorMessage: "Nie udało się edytować filmu" };
    }
  } catch (error) {
    console.log(error);
    return {
      success: false,
      errorMessage: "Nie udało się edytować filmu. Błąd połączenia z serwerem.",
    };
  }
};

const deleteMovie = async (id) => {
  try {
    const movieId = parseInt(id);
    var response = await axiosInstance.delete("/deleteMovie/" + movieId);
    if (response.status === 200) {
      return { success: true };
    } else {
      return { success: false, errorMessage: "Nie udało się usunąć filmu" };
    }
  } catch (error) {
    console.log(error);
    return {
      success: false,
      errorMessage: "Nie udało się usunąć filmu. Błąd połączenia z serwerem.",
    };
  }
};

export default {
  getMovies,
  getNewMovies,
  addMovie,
  editMovie,
  deleteMovie,
};

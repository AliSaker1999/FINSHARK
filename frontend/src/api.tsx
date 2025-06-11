import axios from "axios";
import { CompanySearch } from "./company";

interface SearchResponse {
  data: CompanySearch[];
}

export const searchCompanies = async (query: string) => {
    try {
    const response = await axios.get<SearchResponse>(
      `https://financialmodelingprep.com/api/v3/search?query=${query}&limit=10&exchange=NASDAQ&apikey=${process.env.REACT_APP_API_KEY}`
    );
    return response.data;
    }
    
    catch (error: unknown) {
        if ((error as any).isAxiosError) {
            const axiosError = error as any;
            console.log("Axios error:", axiosError.message);
            return axiosError.message;
         }
        else {
            console.log("Unexpected error:", error);
            return "An unexpected error has occurred";
        }
    }
};

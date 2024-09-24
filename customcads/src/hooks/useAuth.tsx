import { useContext } from "react";
import { AuthContext } from "@/contexts/auth-context";

const useAuth = () => useContext(AuthContext);

export default useAuth;
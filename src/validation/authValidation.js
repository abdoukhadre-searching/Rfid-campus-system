import { check } from "express-validator";

export const validateRegister = [
    check("email", "Email invalide").isEmail().trim(),

    check("password", "Mot de passe invalide. le mot de passe doit contenir au moins 2 caractères")
    .isLength({ min: 2 }),

    check("passwordConfirmation", "Le mot de passe de confirmation est différent")
    .custom((value, { req }) => {
        return value === req.body.password
    })
];

export const validateLogin = [
    check("email", "Email invalide").isEmail().trim(),

    check("password", "Mot de passe invalide")
    .not().isEmpty()
];

// module.exports = {
//     validateRegister: validateRegister,
//     validateLogin: validateLogin
// };
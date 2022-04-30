import {connection} from "../configs/DBConnection.js";
import bcrypt from "bcryptjs";

export const _handleLogin  = (email, password) => {
    return new Promise(async (resolve, reject) => {
        //check email is exist or not
        let user = await findUserByEmail(email);
        if (user) {
            //compare password
            await bcrypt.compare(password, user.password).then((isMatch) => {
                if (isMatch) {
                    resolve(true);
                } else {
                    reject(`Le mot de passe que vous avez saisi est incorrecte`);
                }
            });
        } else {
            reject(`Cet email "${email}" n'existe pas`);
        }
    });
};


export const  findUserByEmail = (email) => {
    return new Promise((resolve, reject) => {
        try {
            connection.query(
                ' SELECT * FROM `compteetudiant` WHERE `email` = ?  ', email,
                function(err, rows) {
                    if (err) {
                        reject(err)
                    }
                    let user = rows[0];
                    resolve(user);
                }
            );
        } catch (err) {
            reject(err);
        }
    });
};

export const findUserById = (id) => {
    return new Promise((resolve, reject) => {
        try {
            connection.query(
                ' SELECT * FROM `compteetudiant` WHERE `idCompteEtudiant` = ?  ', id,
                function(err, rows) {
                    if (err) {
                        reject(err)
                    }
                    let user = rows[0];
                    resolve(user);
                }
            );
        } catch (err) {
            reject(err);
        }
    });
};

export const comparePassword = (password, userObject) => {
    return new Promise(async (resolve, reject) => {
        try {
            await bcrypt.compare(password, userObject.password).then((isMatch) => {
                if (isMatch) {
                    resolve(true);
                } else {
                    resolve(`Le mot de passe que vous avez saisi est incorrecte`);
                }
            });
        } catch (e) {
            reject(e);
        }
    });
};

// module.exports = {
//     handleLogin: handleLogin,
//     findUserByEmail: findUserByEmail,
//     findUserById: findUserById,
//     comparePassword: comparePassword
// };
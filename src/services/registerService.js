import {connection} from "./../configs/DBConnection.js"
import bcrypt from "bcryptjs"

export const _createNewUser = (data) => {
    return new Promise(async (resolve, reject) => {
        // check if email, contact and code already exist or not
        const isEmailExist = await checkExistEmail(data.email)
        const ifCodeExist = await checkExistCode(data.codePermanent)
        //const ifContactExist = await checkExistContact(data.contact)

        if (isEmailExist) {
            reject(`L'email "${data.email}" existe déjà. Veuillez tenter un autre `)
        } else if (ifCodeExist == false) {
            reject(`Oups! ce code permanent "${data.codePermanent}" n'est pas dans notre base de données`)
        /*
        } else if (ifContactExist == false) {
            reject(`Le numéro de téléphone que vous avez saisi c'est-à-dire le +221 "${data.contact}" n'est pas le votre. Veuiller entrer votre contact exact et identique lors des inscriptions !! `)
        */
        } else {
            // Hasher le mot de passe
            const salt = bcrypt.genSaltSync(10)
            const userdata = {
                email: data.email,
                password: bcrypt.hashSync(data.password, salt),
                codePermanent: data.codePermanent,
                etatCompte: true
            }
            // Créer un nouveau compte pour l'etudiant
            connection.query(' INSERT INTO compteetudiant set ? ', userdata, (err, rows) =>{
                    if (err) {
                        reject(false)
                    }
                    resolve("Utilisateur crée avec succés !")
                   // console.log(data)
                }
            )
        }
    })
}

export const checkExistEmail = (email) => {
    return new Promise( (resolve, reject) => {
        try {
            connection.query(
                ' SELECT * FROM `compteetudiant` WHERE `email` = ?  ', email,
                function(err, resultat) {
                    if (err) {
                        reject(err)
                    }
                    if (resultat.length > 0) {
                        resolve(true)
                    } else {
                        resolve(false)
                    }
                }
            )
        } catch (err) {
            reject(err)
        }
    })
}

export const checkExistCode = (codePermanent) => {
    return new Promise( (resolve, reject) => {
        try {
            const sql = "SELECT * FROM `etudiant` WHERE codePermanent = '"+ codePermanent +"'"
            connection.query(sql ,(err, result) => {
                    if (err) {
                        reject(err)
                    }
                    if (result.length > 0 ) {
                        resolve(true)
                    } else {
                        resolve(false)
                    }
                } 
            )
        } catch (err) {
            reject(err)
        }
    })
}
/*
const checkExistContact = (contact) => {
    return new Promise( (resolve, reject) => {
        try {
            const sql = "SELECT * FROM `etudiant` WHERE telephone = ' "+ contact +"' "
            DBConnection.query(sql ,(err, result) => {
                    if (err) {
                        reject(err)
                    }
                    if (result.length > 0 ) {
                        resolve(true)
                    } else {
                        resolve(false)
                    }
                } 
            )
        } catch (err) {
            reject(err)
        }
    })
}
*/
// module.exports = {
//     _createNewUser: _createNewUser
// }
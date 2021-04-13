import DBConnection from "./../configs/DBConnection";
import bcrypt from "bcryptjs";

const createNewUser = (data) => {
    return new Promise(async (resolve, reject) => {
        // check if email and code already exist or not
        const isEmailExist = await checkExistEmail(data.email);
        const ifCodeExist = await checkExistCodePermanent(data.codePermanent);

        if (ifCodeExist == false) {
            reject(`Ce code permanent "${data.codePermanent}" n'existe pas dans notre base de données`);
        }
        
        if (ifCodeExist == true) {
            reject(`Ce code permanent "${data.codePermanent}" est déjà associé à un compte`);
        } 
        if (isEmailExist) {
            reject(`L'email "${data.email}" existe déjà. Veuillez tenter un autre `);
        } else { 
            // hash password
            let salt = bcrypt.genSaltSync(10);
            let userItem = { 
                codePermanent: data.codePermanent,
                email: data.email,
                password: bcrypt.hashSync(data.password, salt),
            };

            //créer un nouveau compte
            DBConnection.query(
                ' INSERT INTO compte set ? ', userItem,
                (err, rows) =>{
                    if (err) {
                        reject(false)
                    }
                    resolve("Utilisateur crée avec succés !");
                   // console.log(data)
                }
            );
        }
    });
};

const checkExistEmail = (email) => {
    return new Promise( (resolve, reject) => {
        try {
            DBConnection.query(
                ' SELECT * FROM `compte` WHERE `email` = ?  ', email,
                function(err, rows) {
                    if (err) {
                        reject(err)
                    }
                    if (rows.length > 0) {
                        resolve(true)
                    } else {
                        resolve(false)
                    }
                }
            );
        } catch (err) {
            reject(err);
        }
    });
}

const checkExistCodePermanent = (codePermanent) => {
    return new Promise( (resolve, reject) => {
        try {
            const sql = "SELECT (codePermanent) FROM `etudiants` WHERE codePermanent = ' "+ codePermanent +" ' "
            DBConnection.query(sql ,(err, resultat) => {
                    if (err) {
                        reject(err)
                    }
                    if (resultat.length > 0) {
                        resolve(true) /* on souhaite poursuivre l'inscription s'il le code permanent
                                        n'est pas au préalable dans la base de données ""ET"" associé a un nom ou prénom */
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
module.exports = {
    createNewUser: createNewUser
};
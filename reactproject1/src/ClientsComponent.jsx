import { useEffect, useState } from "react";

function ClientsComponent() {
    const [clients, setClients] = useState([])
    useEffect(() => {
        const fetchClients = async() => {
            const response = await fetch("https://localhost:7202/api/ClientsAPI")
            const data = await  response.json()
            setClients(data)
        }
        fetchClients()
    }, [])
  return (
      <div>
          <table>
              <thead>
                  <tr>
                      <th>cine</th>
                      <th>Nom</th>
                      <th>Prenom</th>
                      <th>Tel</th>
                      <th>Adresse</th>
                  </tr>
              </thead>
              <tbody>
          {clients.map(client => (
              <tr key={client.cine}>
                  <td>{client.cine}</td>
                  <td>{client.nom}</td>
                  <td>{client.prenom}</td>
                  <td>{client.tel}</td>
                  <td>{client.adresse}</td>
              </tr>
          ))}   
          </tbody>
          </table>
      </div>
  );
}

export default ClientsComponent;
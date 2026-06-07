import { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [voitures, setVoitures] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchVoitures = async () => {
      try {
        const response = await fetch("https://localhost:7202/api/VoituresAPI")
        if (response.ok) {
          const data = await response.json()
          setVoitures(data)
        } else {
          console.error("Erreur lors de la récupération des voitures")
        }
      } catch (error) {
        console.error("Erreur réseau:", error)
      } finally {
        setLoading(false)
      }
    }
    fetchVoitures()
  }, [])

  return (
    <div className="app-container">
      <header className="app-header">
        <h1>RentACar <span className="highlight">React Frontend</span></h1>
        <p>Découvrez notre flotte Premium depuis notre API ASP.NET Core.</p>
      </header>
      
      <main className="catalog-container">
        {loading ? (
          <div className="loading">Chargement des véhicules...</div>
        ) : voitures.length > 0 ? (
          <div className="voitures-grid">
            {voitures.map(voiture => (
              <div className="voiture-card glass-panel" key={voiture.matricule}>
                <div className="voiture-image-container">
                  {voiture.imageUrl ? (
                    <img src={voiture.imageUrl.startsWith('/') ? `https://localhost:7202${voiture.imageUrl}` : voiture.imageUrl} alt={`${voiture.marque} ${voiture.modele}`} />
                  ) : (
                    <div className="no-image-placeholder">
                      <span>{voiture.categorie === 'SUV' ? '🚙' : '🚗'}</span>
                    </div>
                  )}
                  <div className={`status-badge ${voiture.etatDisponibilite === 'Disponible' ? 'status-dispo' : 'status-louee'}`}>
                    {voiture.etatDisponibilite}
                  </div>
                </div>
                <div className="voiture-details">
                  <h3>{voiture.marque} {voiture.modele}</h3>
                  <div className="voiture-meta">
                    <span className="meta-item">📅 {voiture.annee}</span>
                    <span className="meta-item">🏷️ {voiture.categorie}</span>
                  </div>
                  <div className="price-tag">
                    <span className="price">{voiture.prixLocation} MAD</span>
                    <span className="price-period">/jour</span>
                  </div>
                </div>
              </div>
            ))}
          </div>
        ) : (
          <div className="empty-state">Aucune voiture disponible.</div>
        )}
      </main>
    </div>
  )
}

export default App

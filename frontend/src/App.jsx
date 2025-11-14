import React from 'react';

export default function App() {
  const openPopup = (path) => {
    const popup = window.open(path, 'sso', 'width=600,height=700');
    if (!popup) alert('Popup blocked. Allow popups for this site.');
  }

  return (
    <div style={{padding:20,fontFamily:'Arial'}}>
      <h1>Clean SSO Demo (React)</h1>
      <div style={{display:'flex',gap:12}}>
        <button onClick={() => openPopup('https://localhost:7243/account/login-google')}>Login with Google</button>
        <button onClick={() => openPopup('https://localhost:7243/account/login-facebook')}>Login with Facebook</button>
      </div>
    </div>
  )
}

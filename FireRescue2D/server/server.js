import express from 'express';
import cors from 'cors';
import morgan from 'morgan';
import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';
import { nanoid } from 'nanoid';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const app = express();
const PORT = process.env.PORT || 3000;

const WEB_DIR = path.resolve(__dirname, '../web');
const DATA_DIR = path.resolve(__dirname, './data');
const LEADERBOARD_FILE = path.join(DATA_DIR, 'leaderboard.json');

if (!fs.existsSync(DATA_DIR)) fs.mkdirSync(DATA_DIR, { recursive: true });
if (!fs.existsSync(LEADERBOARD_FILE)) fs.writeFileSync(LEADERBOARD_FILE, '[]');

app.use(cors());
app.use(express.json());
app.use(morgan('dev'));

// Static frontend
app.use(express.static(WEB_DIR));

// In-memory game state (last seen)
let lastState = null;

app.post('/api/state', (req, res) => {
  lastState = { ...req.body, ts: Date.now() };
  res.json({ ok: true });
});

app.get('/api/state', (req, res) => {
  res.json(lastState || {});
});

app.get('/api/leaderboard', (req, res) => {
  const list = JSON.parse(fs.readFileSync(LEADERBOARD_FILE, 'utf8'));
  list.sort((a, b) => b.score - a.score);
  res.json(list.slice(0, 50));
});

app.post('/api/leaderboard', (req, res) => {
  const { name, score } = req.body || {};
  const entry = { id: nanoid(10), name: (name || 'Anon').slice(0, 32), score: Number(score) || 0, ts: Date.now() };
  const list = JSON.parse(fs.readFileSync(LEADERBOARD_FILE, 'utf8'));
  list.push(entry);
  fs.writeFileSync(LEADERBOARD_FILE, JSON.stringify(list, null, 2));
  res.json({ ok: true, entry });
});

app.listen(PORT, () => {
  console.log(`Server running at http://localhost:${PORT}`);
});


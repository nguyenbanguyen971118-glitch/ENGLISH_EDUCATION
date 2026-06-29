import React from 'react';
import Phonetics from './Phonetics';
import MultipleChoice from './MultipleChoice';
import ListeningMultipleChoice from './ListeningMultipleChoice';
import ListeningDictation from './ListeningDictation';
import Trios from './Trios';
import ListeningSpeakerMatching from './ListeningSpeakerMatching';
import Reading from './Reading';
import FillBlank from './FillBlank';
import WordForm from './WordForm';
import WordBank from './WordBank';
import MatchingHeadings from './MatchingHeadings';
import InsertSentence from './InsertSentence';
import ErrorCorrection from './ErrorCorrection';
import SentenceRewrite from './SentenceRewrite';
import EssayWriting from './EssayWriting';
import OpenCloze from './OpenCloze';
import ReadingShortAnswer from './ReadingShortAnswer';
import ClosestMeaning from './ClosestMeaning';
import IdiomExplanation from './IdiomExplanation';

const QuestionTypes = ({ 
  question, 
  currentQuestion, 
  answers, 
  onAnswerChange, 
  currentSubIndex
}) => {
  const answer = answers[currentQuestion];

  switch (question.type) {
    case "phonetics":
      return <Phonetics question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "multiple-choice":
      return <MultipleChoice question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "listening-multiple-choice":
      return <ListeningMultipleChoice question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "listening-dictation":
      return <ListeningDictation 
        question={question} 
        currentQuestion={currentQuestion} 
        answers={answers} 
        onAnswerChange={onAnswerChange} 
      />;
    case "trios":
      return <Trios question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "listening-speaker-matching":
      return <ListeningSpeakerMatching 
        question={question} 
        currentQuestion={currentQuestion} 
        answers={answers} 
        onAnswerChange={onAnswerChange} 
      />;
    case "reading":
      return <Reading 
        question={question} 
        currentQuestion={currentQuestion} 
        currentSubIndex={currentSubIndex} 
        answers={answers} 
        onAnswerChange={onAnswerChange} 
      />;
    case "fill-blank":
      return <FillBlank question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "word-form":
      return <WordForm question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "word-bank":
      return <WordBank 
        question={question} 
        currentQuestion={currentQuestion} 
        currentSubIndex={currentSubIndex} 
        answers={answers} 
        onAnswerChange={onAnswerChange} 
      />;
    case "matching-headings":
      return <MatchingHeadings 
        question={question} 
        currentQuestion={currentQuestion} 
        currentSubIndex={currentSubIndex} 
        answers={answers} 
        onAnswerChange={onAnswerChange} 
      />;
    case "insert-sentence":
      return <InsertSentence question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "error-correction":
      return <ErrorCorrection 
        question={question} 
        currentQuestion={currentQuestion} 
        answers={answers} 
        onAnswerChange={onAnswerChange} 
      />;
    case "sentence-rewrite":
      return <SentenceRewrite question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "essay-writing":
      return <EssayWriting question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "open-cloze":
      return <OpenCloze 
        question={question} 
        currentQuestion={currentQuestion} 
        currentSubIndex={currentSubIndex} 
        answers={answers} 
        onAnswerChange={onAnswerChange} 
      />;
    case "reading-short-answer":
      return <ReadingShortAnswer question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "closest-meaning":
      return <ClosestMeaning question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    case "idiom-explanation":
      return <IdiomExplanation question={question} currentQuestion={currentQuestion} answer={answer} onAnswerChange={onAnswerChange} />;
    default:
      return <p>Loại câu hỏi không được hỗ trợ.</p>;
  }
};

export default QuestionTypes;

import React, { useState, useEffect } from "react";
import { useTest } from "../../context/TestContext";
import AssignmentList from "../../components/student/AssignmentList";
import AssignmentDetails from "../../components/student/AssignmentDetails";
import TestHeader from "../../components/student/TestHeader";
import QuestionNavigator from "../../components/student/QuestionNavigator";
import QuestionTypes from "../../components/student/question-types/QuestionTypes";
import SubmissionConfirm from "../../components/student/SubmissionConfirm";
import ResultsPage from "../../components/student/ResultsPage";

const StudentAssignmentPage = () => {
  const { setIsTestActive } = useTest();
  const [view, setView] = useState("list");
  const [selectedAssignment, setSelectedAssignment] = useState(null);
  const [currentQuestion, setCurrentQuestion] = useState(0);
  const [answers, setAnswers] = useState({});
  const [markedForReview, setMarkedForReview] = useState({});
  const [timeLeft, setTimeLeft] = useState(60 * 60);
  const [showSubmissionConfirm, setShowSubmissionConfirm] = useState(false);
  const [showResults, setShowResults] = useState(false);

  const assignments = [
    {
      id: 1,
      title: "PRACTICE TEST 48",
      subtitle: "TOEIC Reading giữa khóa",
      questions: 40,
      duration: "60 phút",
      status: "Đang mở",
      statusColor: "#DBEAFE",
      statusTextColor: "#1D4ED8",
      classInfo: "Lớp TOEIC 500 - K12",
      questionData: [
        {
          type: "phonetics",
          title: "Multiple Choice Practice",
          instruction: "Chọn từ có phần gạch chân phát âm khác.",
          hint: "Học viên so sánh âm được gạch chân trong 4 từ và chọn từ có cách phát âm khác 3 từ còn lại.",
          options: ["sacrificed", "increased", "released", "supposed"],
          answer: null,
        },
        {
          type: "phonetics",
          title: "Phonetics Practice",
          instruction: "Chọn từ có phần gạch chân phát âm khác.",
          options: ["washed", "watched", "danced", "visited"],
          answer: null,
        },
        {
          type: "listening-multiple-choice",
          title: "Listening Practice",
          instruction: "Nghe đoạn hội thoại và chọn đáp án đúng.",
          audioUrl: "/tracks/track01.mp3",
          trackName: "Track 01 - Conversation",
          listensLeft: 2,
          question: "How does Tim feel about the changes in the town centre?",
          options: [
            "He regrets they were made so quickly.",
            "He believes they were inevitable.",
            "He thinks the council could have foreseen the problem.",
          ],
          answer: null,
        },
        {
          type: "listening-dictation",
          title: "Listening Dictation",
          instruction: "Nghe audio và điền từ còn thiếu vào đoạn văn.",
          audioUrl: "/tracks/track03.mp3",
          trackName: "Track 03 - Announcement",
          listensLeft: 2,
          rules: [
            "Mỗi ô tối đa 3 từ.",
            "Có thể tự nghe lại tối đa 10 lần đầu.",
            "Không phân biệt hoa/thường.",
            "Từ lưu khi nhập.",
          ],
          passage: "The English club meeting will take place on ______ at ______.",
          blanks: [{ answer: null }, { answer: null }],
        },
        {
          type: "trios",
          title: "Trios",
          instruction: "Tìm một từ phù hợp với cả ba câu.",
          sentences: [
            "There was still ______ for improvement.",
            "This desk takes up too much ______.",
            "There was little ______ for manoeuvre.",
          ],
          answer: null,
          hint: "Lưu Ý: chỉ có một ô đáp án vì cùng một từ dùng cho cả 3 câu. Không tạo 3 ô riêng.",
        },
        {
          type: "listening-speaker-matching",
          title: "Listening Matching",
          instruction: "Nghe 3 speaker và chọn thông tin phù hợp cho từng người.",
          audioUrl: "/tracks/track02.mp3",
          trackName: "Track 02 - Three short extracts",
          listensLeft: 2,
          speakers: [
            { id: 1, answer: null },
            { id: 2, answer: null },
            { id: 3, answer: null },
          ],
          options: [
            "A. Our child began this activity at school.",
            "B. A relative was indirectly responsible.",
            "C. A newspaper article sparked off interest.",
          ],
        },
        {
          type: "multiple-choice",
          question: "The film is ______ based on a true story, but most of it is fiction.",
          options: ["loosely", "casually", "faintly", "lightly"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "The weather was ______ hot that we couldn't go outside.",
          options: ["so", "such", "very", "too"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "I've never ______ such a beautiful sunset before.",
          options: ["see", "saw", "seen", "seeing"],
          answer: null,
        },
        {
          type: "reading",
          passage: `Island Biogeography Model
Since the 1960s, scientists have been studying the composition and stability of island communities. Specifically, they have been interested in what it takes to maintain life in these isolated areas.
The island biogeography model theorizes that the number of different species on an island will maintain equilibrium, or a state of balance, amid the fluid change of immigration and extinction.`,
          questions: [
            {
              question: 'The word "fluid" in the passage is closest in meaning to:',
              options: ["unpredictable", "unstable", "changing", "graceful"],
              answer: null,
            },
            {
              question: 'The word "they" in paragraph 2 refers to:',
              options: ["scientists", "communities", "islands", "species"],
              answer: null,
            },
          ],
        },
        {
          type: "fill-blank",
          question: "Many island communities have increasingly become ______ on tourism as their primary means of earning a living.",
          hint: "Giới hạn: 1 từ",
          answer: null,
        },
        {
          type: "fill-blank",
          question: "The company decided to ______ the meeting until next week.",
          hint: "Giới hạn: 1 từ",
          answer: null,
        },
        {
          type: "word-form",
          question: "______ is what he takes up as a hobby.",
          word: "CARPENTER",
          answer: null,
        },
        {
          type: "word-form",
          question: "The ______ of the project was a great success.",
          word: "DEVELOP",
          answer: null,
        },
        {
          type: "word-bank",
          wordBank: ["bush", "chest", "fingertips", "footsteps", "mouth", "pinch"],
          sentences: [
            {
              text: "Get the load off your ______ and tell us what the matter is.",
              answer: null,
            },
            {
              text: "Arthur seems to be following in his ______.",
              answer: null,
            },
          ],
        },
        {
          type: "matching-headings",
          headings: [
            "i. Using hunting to stop a worse crime",
            "ii. Legal hunting has little financial benefit",
            "iii. Trying to make a living",
            "vi. Emotional reactions may have negative consequences"
          ],
          paragraphs: [
            { label: "Paragraph A" },
            { label: "Paragraph B" },
            { label: "Paragraph C" }
          ],
        },
        {
          type: "insert-sentence",
          sentenceToInsert: "However, while many older inhabitants may lose out to newer species, the number of species on an island will stay the same.",
          passageParts: [
            { text: "As new species immigrate, there begins a competition for resources on the island.", marker: "A" },
            { text: "Since there is a fixed amount of resources on any given island, some species will not survive in the struggle.", marker: "B" },
            { text: "On a smaller island, the rate of extinction would be lower.", marker: "C" },
            { text: "", marker: "D" }
          ]
        },
        {
          type: "error-correction",
          passage: "If scientists were able to tell governments of an impending earthquake, even if only a few hours at advance...",
          errors: [
            { error: null, correction: null },
            { error: null, correction: null },
            { error: null, correction: null },
            { error: null, correction: null }
          ]
        },
        {
          type: "sentence-rewrite",
          originalSentence: "Could I stay with you for the weekend?",
          requiredWord: "PUT",
          placeholder: "Could you put me up for the weekend?"
        },
        {
          type: "essay-writing",
          prompt: "Complete the following sentences to make an essay answering the question: What are the benefits of learning English?"
        },
        {
          type: "open-cloze",
          passage: "Many island communities have increasingly become (1) on tourism as their primary means of earning a living. This has brought money into local economies, but it has also made them (2) on global travel trends. When visitor numbers fall, jobs can quickly disappear.",
          blanks: [{ num: 1 }, { num: 2 }]
        },
        {
          type: "reading-short-answer",
          passage: "Trophy hunters can provide money that supports conservation areas and helps prevent unlawful killing of animals.",
          question: "What do trophy hunters provide that helps prevent unlawful killing of animals?",
          limit: "NO MORE THAN THREE WORDS AND/OR A NUMBER"
        },
        {
          type: "closest-meaning",
          originalSentence: "You are required to hand in your homework on time in order not to get a zero.",
          options: [
            "You must submit your homework on time to avoid getting a zero.",
            "You may submit your homework whenever you want.",
            "You got a zero because you submitted your homework.",
            "You do not have to hand in homework."
          ]
        },
        {
          type: "idiom-explanation",
          idiom: "take the mickey out of somebody"
        },
        {
          type: "trios",
          sentences: [
            "There was still ______ for improvement.",
            "This desk takes up too much ______.",
            "There was little ______ for manoeuvre."
          ],
          hint: "Lưu ý: chỉ có một đáp án và cùng một từ dùng cho cả 3 câu. Không tạo 3 ô riêng."
        },
        {
          type: "listening-multiple-choice",
          trackName: "Track 01 - Conversation",
          instruction: "Số lần nghe còn lại: 2",
          questionText: "How does Tim feel about the changes in the town centre?",
          options: [
            "He regrets they were made so quickly.",
            "He believes they were inevitable.",
            "He thinks the council could have foreseen the problem."
          ]
        },
        {
          type: "listening-dictation",
          trackName: "Track 03 - Announcement",
          listeningHint: "Số lần nghe còn lại: 2 • Có thể tự nghe tối đa 10 lần đầu",
          rules: [
            "Mỗi ô tối đa 3 từ.",
            "Không phân biệt hoa/thường.",
            "Từ lưu khi nhập."
          ],
          passage: "The English club meeting will take place on ______ at ______. In Room 204. Students should bring their notebooks and prepare a short ______ about their favorite city."
        },
        {
          type: "phonetics",
          phoneticType: "sound",
          title: "Chọn từ có phần gạch chân phát âm khác.",
          instruction: "Học viên so sánh âm được gạch chân 4 từ và chọn cách phát âm khác 3 từ lại.",
          words: ["sacrificed", "increased", "released", "supposed"],
          hint: "Ghi chú: dạng này dùng chung layout trắc nghiệm 1 đáp án, nhưng phần hướng dẫn và nội dung câu hỏi khác để học viên biết dùng dạng.",
          options: [
            "sacrificed",
            "increased",
            "released",
            "supposed"
          ]
        },
        {
          type: "phonetics",
          phoneticType: "stress",
          title: "Chọn từ có trọng âm chính khác.",
          instruction: "Học viên chọn vị trí trọng âm khác với các từ còn lại.",
          words: ["committee", "bewilder", "referee", "computer"],
          hint: "Đã bổ sung màn riêng cho dạng trọng âm. Dạng này vẫn chọn 1 đáp án, nhưng hướng dẫn ghi rõ là xét trọng âm.",
          options: [
            "committee",
            "bewilder",
            "referee",
            "computer"
          ]
        },
        {
          type: "multiple-choice",
          question: "They ______ English for 5 years.",
          options: ["learn", "learned", "have learned", "are learning"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "The cat is sleeping ______ the sofa.",
          options: ["on", "in", "at", "for"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "______ you like tea or coffee?",
          options: ["Do", "Does", "Are", "Is"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "My birthday is ______ May 5th.",
          options: ["in", "on", "at", "to"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "I usually ______ up at 7 AM.",
          options: ["get", "gets", "got", "getting"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "She ______ to music every day.",
          options: ["listen", "listens", "listened", "listening"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "We ______ to the park yesterday.",
          options: ["go", "goes", "went", "going"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "He is ______ than his brother.",
          options: ["tall", "taller", "tallest", "more tall"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "This is ______ book I've ever read.",
          options: ["good", "better", "best", "the best"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "Can you help ______?",
          options: ["I", "me", "my", "mine"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "______ pen is this?",
          options: ["Who", "Whose", "Whom", "Which"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "I don't have ______ money.",
          options: ["some", "any", "many", "much"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "There are ______ apples on the table.",
          options: ["some", "any", "many", "little"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "What ______ you doing?",
          options: ["is", "am", "are", "be"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "I ______ TV when the phone rang.",
          options: ["watch", "watched", "was watching", "watching"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "She will ______ here tomorrow.",
          options: ["come", "comes", "came", "coming"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "They ______ already finished their homework.",
          options: ["have", "has", "had", "having"],
          answer: null,
        },
        {
          type: "multiple-choice",
          question: "I wish I ______ speak French fluently.",
          options: ["can", "could", "will", "would"],
          answer: null,
        },
      ],
    },
  ];

  const getQuestionIndexMap = (data) => {
    const map = [];
    data.forEach((q, idx) => {
      if (q.type === "reading") {
        q.questions.forEach((_, subIdx) => {
          map.push({ dataIndex: idx, type: "reading", subIndex: subIdx });
        });
      } else if (q.type === "word-bank") {
        q.sentences.forEach((_, subIdx) => {
          map.push({ dataIndex: idx, type: "word-bank", subIndex: subIdx });
        });
      } else if (q.type === "matching-headings") {
        q.paragraphs.forEach((_, subIdx) => {
          map.push({ dataIndex: idx, type: "matching-headings", subIndex: subIdx });
        });
      } else {
        map.push({ dataIndex: idx, type: q.type, subIndex: null });
      }
    });
    return map;
  };

  const getTotalQuestions = () => {
    if (!selectedAssignment) return 0;
    return getQuestionIndexMap(selectedAssignment.questionData).length;
  };

  const getCurrentQuestionData = () => {
    if (!selectedAssignment) return null;
    const map = getQuestionIndexMap(selectedAssignment.questionData);
    if (currentQuestion >= map.length) return null;
    const mapEntry = map[currentQuestion];
    return {
      ...selectedAssignment.questionData[mapEntry.dataIndex],
      currentSubIndex: mapEntry.subIndex,
      type: mapEntry.type,
    };
  };

  useEffect(() => {
    let timer;
    if (view === "test" && timeLeft > 0 && !showResults) {
      timer = setInterval(() => {
        setTimeLeft((prev) => prev - 1);
      }, 1000);
    }
    return () => clearInterval(timer);
  }, [view, timeLeft, showResults]);

  useEffect(() => {
    setIsTestActive(view === "test" && !showResults);
  }, [view, setIsTestActive, showResults]);

  useEffect(() => {
    if (view === "test" && !showResults) {
      const requestFullscreen = async () => {
        const elem = document.documentElement;
        if (elem.requestFullscreen) {
          await elem.requestFullscreen();
        } else if (elem.webkitRequestFullscreen) {
          await elem.webkitRequestFullscreen();
        } else if (elem.msRequestFullscreen) {
          await elem.msRequestFullscreen();
        }
      };
      requestFullscreen();
    } else {
      const exitFullscreen = async () => {
        if (document.fullscreenElement) {
          if (document.exitFullscreen) {
            await document.exitFullscreen();
          } else if (document.webkitExitFullscreen) {
            await document.webkitExitFullscreen();
          } else if (document.msExitFullscreen) {
            await document.msExitFullscreen();
          }
        }
      };
      exitFullscreen();
    }
  }, [view, showResults]);

  const getQuestionStatus = (index) => {
    if (markedForReview[index]) return "marked";
    if (answers[index] !== undefined && answers[index] !== null && answers[index] !== "") return "answered";
    return "unanswered";
  };

  const handleAnswerChange = (questionIndex, value) => {
    setAnswers((prev) => ({ ...prev, [questionIndex]: value }));
  };

  const toggleMarkForReview = (index) => {
    setMarkedForReview((prev) => ({ ...prev, [index]: !prev[index] }));
  };

  const startTest = () => {
    setView("test");
    setCurrentQuestion(0);
    setAnswers({});
    setMarkedForReview({});
    setTimeLeft(60 * 60);
    setShowResults(false);
  };

  const goToDetails = (assignment) => {
    setSelectedAssignment(assignment);
    setView("details");
  };

  const goToList = () => {
    setView("list");
    setSelectedAssignment(null);
  };

  const handlePrevQuestion = () => {
    if (currentQuestion > 0) {
      setCurrentQuestion((prev) => prev - 1);
    }
  };

  const handleNextQuestion = () => {
    if (currentQuestion < getTotalQuestions() - 1) {
      setCurrentQuestion((prev) => prev + 1);
    }
  };

  if (view === "list") {
    return (
      <AssignmentList
        assignments={assignments}
        onViewDetails={goToDetails}
      />
    );
  }

  if (view === "details" && selectedAssignment) {
    return (
      <AssignmentDetails
        assignment={selectedAssignment}
        onBack={goToList}
        onStartTest={startTest}
      />
    );
  }

  if (view === "test" && selectedAssignment) {
    const totalQuestions = getTotalQuestions();
    const currentQData = getCurrentQuestionData();

    if (showResults) {
      return <ResultsPage />;
    }

    return (
      <div style={{ fontFamily: "'Montserrat', sans-serif", backgroundColor: "#F3F4F6", minHeight: "100vh" }}>
        <TestHeader
          title={selectedAssignment?.title || "PRACTICE TEST 48"}
          timeLeft={timeLeft}
          onSubmitClick={() => setShowSubmissionConfirm(true)}
        />

        <div className="d-flex" style={{ minHeight: "calc(100vh - 80px)" }}>
          <div style={{ flex: 1, padding: "24px", display: "flex", flexDirection: "column" }}>
            <div style={{
              backgroundColor: "#FFFFFF",
              borderRadius: "16px",
              boxShadow: "0 1px 3px rgba(0,0,0,0.05)",
              flex: 1
            }}>
              {currentQData && (
                <QuestionTypes
                  question={currentQData}
                  currentQuestion={currentQuestion}
                  currentSubIndex={currentQData.currentSubIndex}
                  answers={answers}
                  onAnswerChange={handleAnswerChange}
                />
              )}
            </div>

            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginTop: "20px", backgroundColor: "#FFFFFF", padding: "16px 24px", borderRadius: "12px", boxShadow: "0 1px 2px rgba(0,0,0,0.05)" }}>
              <div style={{ display: "flex", gap: "12px" }}>
                <button
                  onClick={handlePrevQuestion}
                  disabled={currentQuestion === 0}
                  style={{
                    padding: "10px 24px",
                    borderRadius: "8px",
                    border: "2px solid #E5E7EB",
                    backgroundColor: "#FFFFFF",
                    color: "#374151",
                    fontSize: "14px",
                    fontWeight: 600,
                    cursor: currentQuestion === 0 ? "not-allowed" : "pointer",
                    opacity: currentQuestion === 0 ? 0.5 : 1
                  }}
                >
                  <i className="bi bi-arrow-left me-2"></i>
                  Câu trước
                </button>
                <button
                  onClick={() => toggleMarkForReview(currentQuestion)}
                  style={{
                    padding: "10px 24px",
                    borderRadius: "8px",
                    border: `2px solid ${markedForReview[currentQuestion] ? "#F59E0B" : "#E5E7EB"}`,
                    backgroundColor: markedForReview[currentQuestion] ? "#FEF3C7" : "#FFFFFF",
                    color: markedForReview[currentQuestion] ? "#92400E" : "#374151",
                    fontSize: "14px",
                    fontWeight: 600,
                    cursor: "pointer"
                  }}
                >
                  <i className={`bi ${markedForReview[currentQuestion] ? "bi-bookmark-check-fill" : "bi-bookmark"} me-2`}></i>
                  {markedForReview[currentQuestion] ? "Đã đánh dấu" : "Đánh dấu xem lại"}
                </button>
              </div>
              <button
                onClick={handleNextQuestion}
                disabled={currentQuestion === totalQuestions - 1}
                style={{
                  padding: "10px 32px",
                  borderRadius: "8px",
                  border: "none",
                  backgroundColor: "#2563EB",
                  color: "#FFFFFF",
                  fontSize: "14px",
                  fontWeight: 600,
                  cursor: currentQuestion === totalQuestions - 1 ? "not-allowed" : "pointer",
                  opacity: currentQuestion === totalQuestions - 1 ? 0.5 : 1
                }}
              >
                Câu sau
                <i className="bi bi-arrow-right ms-2"></i>
              </button>
            </div>

            <div style={{ backgroundColor: "#FFFFFF", padding: "16px 24px", borderRadius: "12px", marginTop: "16px", boxShadow: "0 1px 2px rgba(0,0,0,0.05)" }}>
              <p style={{ fontSize: "14px", color: "#6B7280", margin: 0 }}>
                Ghi chú: khi học viên chọn đáp án, đã chọn đáp án đổi nền xanh nhạt, số câu bên phải chuyển sang trạng thái đã làm và bài tự động lưu.
              </p>
            </div>
          </div>

          <QuestionNavigator
            totalQuestions={totalQuestions}
            currentQuestion={currentQuestion}
            onQuestionClick={setCurrentQuestion}
            getQuestionStatus={getQuestionStatus}
            markedForReview={markedForReview}
            onToggleMarkForReview={toggleMarkForReview}
          />
        </div>

        <SubmissionConfirm
          show={showSubmissionConfirm}
          onClose={() => setShowSubmissionConfirm(false)}
          onConfirm={() => {
            setShowSubmissionConfirm(false);
            setShowResults(true);
          }}
          totalQuestions={totalQuestions}
          answers={answers}
          markedForReview={markedForReview}
        />
      </div>
    );
  }

  return null;
};

export default StudentAssignmentPage;

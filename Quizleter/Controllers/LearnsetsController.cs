using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizleter.Data;
using Quizleter.Models;
using Quizleter.Services.Learnsets;
using Quizleter.Services.Session;
using Quizleter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizleter.Controllers
{
    public class LearnsetsController : Controller
    {
        private readonly QuizleterContext _context;
        private readonly ILearnsetService _learnsetService;
        private readonly ISessionService _sessionService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LearnsetsController(
            QuizleterContext context,
            ILearnsetService learnsetService, 
            ISessionService sessionService, 
            SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _learnsetService = learnsetService;
            _sessionService = sessionService;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Learn(long id)
        {
            var username = User.Identity.Name;
            var learnVocabList = await _learnsetService.GetLearnVocabByLernsetId(id, username);

            var voacbWithLowestValue = _learnsetService.GetRandomSkill(learnVocabList);

            var learnVocabModel = new LearnVocabViewModel
            {
                Definition = voacbWithLowestValue.Vocab.Definition,
                LearnsetId = id,
                VocabId = voacbWithLowestValue.VocabId
            };

            return View(learnVocabModel);
        }

        [HttpPost]
        public IActionResult Evaluation(long id)
        {
            var vocabWithSkills = new List<VocabWithSkillsViewModel>();
            var voabs = _context.Vocab.Where(v => v.LearnsetId == id)
                                      .ToList();

            foreach (var vocab in voabs)
            {
                vocabWithSkills.Add
                    (
                        new VocabWithSkillsViewModel
                        {
                            Vocab = vocab,
                            Skill = _context.Skill.FirstOrDefault(s => s.VocabId == vocab.Id).SkillLevel
                        }
                    );
            }

            return View("Evaluation", vocabWithSkills);
        }

        [HttpPost]
        public async Task<IActionResult> Learn(LearnVocabViewModel learnVocabViewModel)
        {
            var vocab = _context.Vocab.FirstOrDefault(v => v.Id == learnVocabViewModel.VocabId);
            var skill = _context.Skill.FirstOrDefault(s => s.VocabId == learnVocabViewModel.VocabId);
            if (string.Equals(vocab.Term, learnVocabViewModel.Input))
            {
                skill.SkillLevel += 1;
                _context.Skill.Update(skill);
                await _context.SaveChangesAsync();
            }
            else
            {
                if (skill.SkillLevel > 0)
                {
                    skill.SkillLevel -= 1;
                }
                _context.Skill.Update(skill);
                await _context.SaveChangesAsync();
            }

            var username = User.Identity.Name;
            var learnVocabList = await _learnsetService.GetLearnVocabByLernsetId(learnVocabViewModel.LearnsetId, username);

            var voacbWithLowestValue = _learnsetService.GetRandomSkill(learnVocabList);

            var learnVocabModel = new LearnVocabViewModel
            {
                Definition = voacbWithLowestValue.Vocab.Definition,
                LearnsetId = learnVocabViewModel.LearnsetId,
                VocabId = voacbWithLowestValue.VocabId
            };

            return View(learnVocabModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index(LearnsetsViewModel viewModel)
        {
            var result = new LearnsetsViewModel();

            if (!_signInManager.IsSignedIn(User))
            {
                result.OtherLearnsets = (await _learnsetService
                    .GetAllLearnsetsAsync())
                    .ToList();

                return View(result);
            }

            result.OwnedLearnsets = (await _learnsetService
                .GetLearnsetsByUserAsync(User.Identity.Name))
                .ToList();

            result.OtherLearnsets = (await _learnsetService
                .GetAllLearnsetsAsync())
                .Where(l => !l.CreatorUsername.Equals(User.Identity.Name))
                .ToList();

            if (viewModel.SearchText != null)
            {
                var normalizedSearchText = viewModel.SearchText.ToLower();

                result.OtherLearnsets = result.OtherLearnsets
                    .Where(l => l.Name.ToLower().Contains(normalizedSearchText)
                                || l.Desc.ToLower().Contains(normalizedSearchText)
                                || l.CreatorUsername.ToLower().Contains(normalizedSearchText))
                    .ToList();
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var learnset = await _context.Learnset.FindAsync(id);
            if (learnset == null)
            {
                return NotFound();
            }

            var result = new LearnsetDetailsViewModel
            {
                Id = learnset.Id,
                Name = learnset.Name,
                Description = learnset.Desc,
                Creator = learnset.CreatorUsername,
                VocabCount = _context.Vocab.Count(v => v.LearnsetId == learnset.Id)
            };

            return View(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(CreateLearnsetViewModel viewModel)
        {
            if (viewModel.Vocabulary == null)
            {
                _sessionService.ClearSession();
                return View(new CreateLearnsetViewModel
                {
                    Vocabulary = new List<Vocab>()
                });
            }

            if (string.IsNullOrWhiteSpace(viewModel.NewDefinition))
            {
                ModelState.AddModelError(nameof(viewModel.NewDefinition), "The definition can't empty.");
            }
            if (string.IsNullOrWhiteSpace(viewModel.NewTerm))
            {
                ModelState.AddModelError(nameof(viewModel.NewTerm), "The term can't empty.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return View(viewModel);
            }

            var vocabulary = new List<Vocab>();

            if (_sessionService.KeyExists("vocabulary"))
            {
                vocabulary = _sessionService.GetValue<List<Vocab>>("vocabulary");
            }

            vocabulary.Add(new Vocab
            {
                Definition = viewModel.NewDefinition,
                Term = viewModel.NewTerm
            });
            viewModel.Vocabulary = vocabulary;

            _sessionService.StoreValue("vocabulary", vocabulary);

            viewModel.NewDefinition = string.Empty;
            viewModel.NewTerm = string.Empty;

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost([Bind("Title,Description")] CreateLearnsetViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Title))
            {
                ModelState.AddModelError(nameof(viewModel.Title), "The title can't be empty.");
            }
            if (string.IsNullOrWhiteSpace(viewModel.Description))
            {
                ModelState.AddModelError(nameof(viewModel.Description), "The description can't be empty.");
            }
            if (ModelState.ErrorCount > 0)
            {
                if (viewModel.Vocabulary == null)
                {
                    viewModel.Vocabulary = new List<Vocab>();
                }
                return View(nameof(Create), viewModel);
            }

            var learnset = new Learnset
            {
                Name = viewModel.Title,
                Desc = viewModel.Description,
                CreatorUsername = User.Identity.Name
            };

            var vocabulary = _sessionService.GetValue<List<Vocab>>("vocabulary");
            if (!vocabulary.Any())
            {
                ModelState.AddModelError(nameof(viewModel.Vocabulary), "There's no vocabulary in your learnset.");
                return View(nameof(Create), viewModel);
            }

            foreach (var vocab in vocabulary)
            {
                vocab.Learnset = learnset;
            }

            _context.Learnset.Add(learnset);
            _context.Vocab.AddRange(vocabulary);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var learnset = await _context.Learnset.FindAsync(id);
            if (learnset == null)
            {
                return NotFound();
            }

            var username = User.Identity.Name;
            if (!learnset.CreatorUsername.Equals(username))
            {
                return Unauthorized();
            }

            var result = new EditLearnsetViewModel
            {
                Id = learnset.Id,
                Name = learnset.Name,
                Desc = learnset.Desc
            };

            return View(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditLearnsetViewModel viewModel)
        {
            var learnset = await _context.Learnset.FindAsync(viewModel.Id);
            var username = User.Identity.Name;
            if (learnset == null || !learnset.CreatorUsername.Equals(username))
            {
                return NotFound();
            }

            learnset.Name = viewModel.Name;
            learnset.Desc = viewModel.Desc;

            _context.Learnset.Update(learnset);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Cards(long id)
        {
            var vocabsOfLearnsets = await _context.Vocab
                .Where(v => v.LearnsetId == id)
                .ToListAsync();

            if (vocabsOfLearnsets == null)
            {
                return NotFound();
            }

            return View(vocabsOfLearnsets);
        }

        [HttpGet]
        public async Task<IActionResult> Test(long id)
        {
            var vocabularyOfLearnset = await _context.Vocab
                .Where(v => v.LearnsetId == id)
                .ToListAsync();

            if (vocabularyOfLearnset.Count < 1)
            {
                return NotFound();
            }

            _sessionService.ClearSession();

            var result = new TestLearnsetViewModel
            {
                LearnsetId = id,
                Index = 0,
                Definition = vocabularyOfLearnset[0].Definition
            };

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Test(TestLearnsetViewModel viewModel)
        {
            var testAnswers = new List<string>();
            if (_sessionService.KeyExists("testAnswers"))
            {
                testAnswers = _sessionService.GetValue<List<string>>("testAnswers");
            }

            testAnswers.Add(viewModel.Input);
            _sessionService.StoreValue("testAnswers", testAnswers);

            viewModel.Index++;
            viewModel.Input = string.Empty;

            var vocabularyOfLearnset = await _context.Vocab
                .Where(l => l.LearnsetId == viewModel.LearnsetId)
                .ToListAsync();
            
            try
            {
                viewModel.Definition = vocabularyOfLearnset[viewModel.Index].Definition;
            }
            catch (ArgumentOutOfRangeException)
            {
                return await TestResult(viewModel.LearnsetId);
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TestResult(long learnsetId)
        {
            var answers = _sessionService.GetValue<List<string>>("testAnswers");
            var vocabularyOfLearnset = await _context.Vocab
                .Where(v => v.LearnsetId == learnsetId)
                .ToListAsync();

            var result = new TestResultViewModel
            {
                Vocabulary = new List<TestVocabViewModel>()
            };

            for (int i = 0; i < vocabularyOfLearnset.Count; i++)
            {
                result.Vocabulary.Add(new TestVocabViewModel
                {
                    Definition = vocabularyOfLearnset[i].Definition,
                    Term = vocabularyOfLearnset[i].Term,
                    Answer = answers[i]
                });
            }

            result.Points = result.Vocabulary.Count(v => v.Term.Equals(v.Answer));
            return View("TestResult", result);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnset = await _context.Learnset
                .FirstOrDefaultAsync(m => m.Id == id);
            if (learnset == null)
            {
                return NotFound();
            }

            return View(learnset);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var learnset = await _context.Learnset.FindAsync(id);
            _context.Learnset.Remove(learnset);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnsetExists(long id)
        {
            return _context.Learnset.Any(e => e.Id == id);
        }
    }
}
